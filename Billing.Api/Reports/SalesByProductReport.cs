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
    public class SalesByProductReport
    {
        private BillingIdentity identity = new BillingIdentity();
        private ReportFactory Factory = new ReportFactory();
        private UnitOfWork _unitOfWork;
        public SalesByProductReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public SalesByProductModel Report(RequestModel Request)
        {
            var Items = _unitOfWork.Items.Get().Where(x => x.Invoice.Date >= Request.StartDate && x.Invoice.Date <= Request.EndDate).ToList();
            var ItemsOfProduct = _unitOfWork.Items.Get().Where(x => x.Invoice.Date >= Request.StartDate && x.Invoice.Date <= Request.EndDate && x.Product.Category.Id == Request.Id).ToList();
            Category Category = _unitOfWork.Categories.Get(Request.Id);
            double grandTotal = Items.Sum(x => x.SubTotal);
            double categoryTotal = ItemsOfProduct.Sum(x => x.SubTotal);


            SalesByProductModel result = new SalesByProductModel()
            {
                CategoryName = Category.Name,
                StartDate = Request.StartDate,
                EndDate = Request.EndDate,
                CategoryTotal = Math.Round(grandTotal, 2),
                PercentTotal = Math.Round(100 * categoryTotal / grandTotal, 2)
            };

            result.Products = _unitOfWork.Products.Get().Where(x => x.Category.Id == Request.Id).ToList()
                                        .Select(x => Factory.Create(x.Name,x.Price, categoryTotal,grandTotal))
                                        .ToList();









            return result;
        }
    }
}