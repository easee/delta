using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Billing.Api.Reports
{
    public class DashboardReport
    {
        private BillingIdentity identity = new BillingIdentity();
        private ReportFactory Factory = new ReportFactory();
        private UnitOfWork _unitOfWork;
        public DashboardReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DashboardModel Report()
        {
            int currentMonth = Convert.ToInt32(ConfigurationManager.AppSettings["currentMonth"]);
            DashboardModel result = new DashboardModel(Helper.StatusCount, Helper.RegionCount);

            result.Title = "Dashboard for " + identity.CurrentUser;

            result.RegionsMonth = _unitOfWork.Invoices.Get()
                    .Where(x => x.Date.Month == currentMonth).ToList()
                    .GroupBy(x => x.Customer.Town.Region)
                    .OrderBy(x => x.Key)
                    .Select(x => Factory.Create(x.Key, x.Sum(y => y.SubTotal))).ToList();

            List<InputItem> query;

            query = _unitOfWork.Invoices.Get()
                    .OrderBy(x => x.Customer.Town.Region).ToList()
                    .GroupBy(x => new { x.Customer.Town.Region, x.Date.Month })
                    .Select(x => new InputItem { Label = x.Key.Region.ToString(), Index = x.Key.Month, Value = x.Sum(y => y.SubTotal) })
                    .ToList();
            result.RegionsYear = Factory.Create(query);

            query = _unitOfWork.Items.Get()
                    .OrderBy(x => x.Product.Category.Id).ToList()
                    .GroupBy(x => new { x.Product.Category.Name, x.Invoice.Date.Month })
                    .Select(x => new InputItem { Label = x.Key.Name, Index = x.Key.Month, Value = x.Sum(y => y.SubTotal) })
                    .ToList();
            result.CategoriesYear = Factory.Create(query);

            query = _unitOfWork.Invoices.Get()
                    .OrderBy(x => x.Agent.Id).ToList()
                    .GroupBy(x => new { agent = x.Agent.Name, region = x.Customer.Town.Region })
                    .Select(x => new InputItem { Label = x.Key.agent, Index = (int)x.Key.region, Value = x.Sum(y => (y.SubTotal)) })
                    .ToList();
            result.AgentsSales = Factory.Create(query, Helper.RegionCount);

            result.Top5Products = _unitOfWork.Items.Get().OrderBy(x => x.Product.Id).ToList()
                                  .GroupBy(x => x.Product.Name)
                                  .Select(x => new ProductSales()
                                  { Product = x.Key, Quantity = x.Sum(y => y.Quantity), Revenue = x.Sum(y => y.SubTotal) })
                                  .OrderByDescending(x => x.Revenue).Take(5)
                                  .ToList();

            result.Invoices = _unitOfWork.Invoices.Get().OrderBy(x => x.Status).ToList()
                              .GroupBy(x => x.Status.ToString())
                              .Select(x => new InvoiceStatus() { Status = x.Key, Count = x.Count() })
                              .ToList();

            var custList = _unitOfWork.Invoices.Get()
                           .OrderBy(x => x.Customer.Id).ToList()
                           .GroupBy(x => new { x.Customer.Name, x.Status })
                           .Select(x => new InputItem()
                           {
                               Label = x.Key.Name,
                               Index = (int)x.Key.Status,
                               Value = x.Sum(y => y.Total)
                           })
                           .ToList();
            result.Customers = Factory.Customers(custList);

            result.BurningItems = _unitOfWork.Products.Get().ToList()
                                  .Select(x => new BurningModel()
                                  {
                                      Id = x.Id,
                                      Name = x.Name,
                                      Stock = (x.Stock != null) ? (int)x.Stock.Inventory : 0,
                                      Sold = (x.Stock != null) ? (int)x.Stock.Output : 0
                                  })
                                  .OrderByDescending(x => x.Sold).Take(5)
                                  .ToList();
            return result;
        }
    }
}