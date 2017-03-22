using Billing.Api.Controllers;
using Billing.Api.Models;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class SalesByCategoryReport
    {

        public static SalesByCategoryModel Report(UnitOfWork UnitOfWork, RequestModel Request)
        {
            SalesByCategoryModel result = new SalesByCategoryModel();

            result.StartDate = Request.StartDate;
            result.EndDate = Request.EndDate;

            var Items = UnitOfWork.Items.Get().Where(x => x.Invoice.Date >= Request.StartDate && x.Invoice.Date <= Request.EndDate).ToList();
            result.GrandTotal = Items.Sum(x => x.Invoice.Total);

            result.Categories = new List<CategoriesSalesModel>();
            var query = Items.GroupBy(x => new { name = x.Product.Category.Name })
                              .Select(x => new { CategoryName = x.Key.name, CategoryTotal = x.Sum(y => y.Invoice.Total) }).ToList();

            foreach (var item in query)
            {
                result.Categories.Add(new CategoriesSalesModel()
                {
                    CategoryName = item.CategoryName,
                    CategoryTotal = Math.Round(item.CategoryTotal, 2),
                    CategoryPercent = Math.Round(100 * item.CategoryTotal / result.GrandTotal, 2)

                });
            }

            return result;

        }
        
    }
}