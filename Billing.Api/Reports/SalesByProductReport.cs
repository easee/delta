using Billing.Api.Controllers;
using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Repository;
using System;
using Billing.Database;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class SalesByProductReport : BaseReport
    {
        public SalesByProductReport(UnitOfWork unitOfWork) : base(unitOfWork) { }

        public SalesByProductModel Report(RequestModel Request)
        {
            var Items = UnitOfWork.Items.Get().Where(x => x.Invoice.Date >= Request.StartDate && x.Invoice.Date <= Request.EndDate).ToList();
            var ItemsOfProduct = UnitOfWork.Items.Get().Where(x => x.Invoice.Date >= Request.StartDate && x.Invoice.Date <= Request.EndDate && x.Product.Category.Id == Request.Id).ToList();
            Category Category = UnitOfWork.Categories.Get(Request.Id);
            double grandTotal = Items.Sum(x => x.SubTotal);
            double categoryTotal = ItemsOfProduct.Sum(x => x.SubTotal);


            SalesByProductModel result = new SalesByProductModel()
            {
                CategoryName = Category.Name,
                StartDate = Request.StartDate,
                EndDate = Request.EndDate,
                CategoryTotal = Math.Round(categoryTotal, 2),
                PercentTotal = Math.Round(100 * categoryTotal / grandTotal, 2)
            };

            result.Products = Items.Where(x => x.Product.Category.Id == Request.Id)
                                   .GroupBy(x => x.Product.Name)
                                   .Select(x => Factory.Create(x.Key, x.Sum(y => y.SubTotal), categoryTotal, grandTotal))
                                   .ToList();

            return result;
        }
    }
}