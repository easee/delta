using Billing.Api.Controllers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class SalesByCustomerReport
    {
        public static SalesByCustomerModel Report(UnitOfWork UnitOfWork, RequestModel Request)
        {
            SalesByCustomerModel result = new SalesByCustomerModel();

            result.StartDate = Request.StartDate;
            result.EndDate = Request.EndDate;

            var Invoices = UnitOfWork.Invoices.Get().Where(x => x.Date >= Request.StartDate && x.Date <= Request.EndDate).ToList();
            result.GrandTotal = Invoices.Sum(x => x.Total);

            result.Sales = new List<CustomerSalesModel>();
            var customers = Invoices.OrderByDescending(x => x.Total)
                             .GroupBy(x => new { name = x.Customer.Name })
                             .Select(x => new { CustomerName = x.Key.name, CustomerTurnover = x.Sum(y => y.Total) })
                             .ToList();

            foreach (var item in customers)
            {
                result.Sales.Add(new CustomerSalesModel()
                {
                    CustomerName = item.CustomerName,
                    CustomerTurnover = Math.Round(item.CustomerTurnover,2),
                    CustomerPercent = Math.Round(100 * item.CustomerTurnover / result.GrandTotal, 2)
                });
               
            }

            return result;
        }
    }
}