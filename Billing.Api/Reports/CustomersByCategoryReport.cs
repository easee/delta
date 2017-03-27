using Billing.Api.Controllers;
using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class CustomersByCategoryReport
    {
        private BillingIdentity identity = new BillingIdentity();
        private ReportFactory Factory = new ReportFactory();
        private UnitOfWork _unitOfWork;
        public CustomersByCategoryReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public  CustomerByCategoryModel Report(RequestModel Request)
        {
            var Items = _unitOfWork.Items.Get().Where(x => x.Invoice.Date >= Request.StartDate && x.Invoice.Date <= Request.EndDate).ToList();
          
            CustomerByCategoryModel result = new CustomerByCategoryModel()
            {
                StartDate = Request.StartDate,
                EndDate = Request.EndDate,

                GrandTotal = Items.Sum(x => x.SubTotal)
            };
        
            //result.Sales = new List<CategoryPurchaseModel>();
            //var query = Items.GroupBy(x => x.Product.Category.Name)
            //                    .Select(x => new { CategoryName = x.Key, CategoryTotal = x.Sum(y => y.Invoice.Total) })
            //                    .ToList();


            //foreach (var item in query)
            //{
            //    CategoryPurchaseModel category = new CategoryPurchaseModel()
            //    {
            //        CategoryName = item.CategoryName,
            //        CategoryTotal = Math.Round(item.CategoryTotal, 2)
            //    };

            //    category.Customers = new List<CustomerPurchaseModel>();
            //    var customers = Items.Where(x => x.Product.Category.Name == item.CategoryName)
            //                         .GroupBy(x => new { id = x.Invoice.Customer.Id, name = x.Invoice.Customer.Name })
            //                         .Select(x => new { CustomerId = x.Key.id, CustomerName = x.Key.name, CustomerTurnover = x.Sum(y => y.SubTotal) })
            //                         .ToList();
            //    foreach (var customer in customers)
            //    {
            //        category.Customers.Add(new CustomerPurchaseModel()
            //        {
            //            CustomerId = customer.CustomerId,
            //            CustomerName = customer.CustomerName,
            //            CustomerTurnover = customer.CustomerTurnover,
            //            //CategoryPercent = Math.Round(100 * customer.CustomerTotal / category.CategoryTotal, 2),
            //            //TotalPercent = Math.Round(100 * customer.CustomerTotal / result.GrandTotal, 2)
            //        });
            //    }
            //    result.Sales.Add(category);
            //}

            return result;
        }
    }
}