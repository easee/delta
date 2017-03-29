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
    public class SalesByCategoryReport
    {
        private BillingIdentity identity = new BillingIdentity();
        private ReportFactory Factory = new ReportFactory();
        private UnitOfWork _unitOfWork;
        public SalesByCategoryReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public SalesByCategoryModel Report (RequestModel Request)
        {
            SalesByCategoryModel result = new SalesByCategoryModel();

            result.StartDate = Request.StartDate;
            result.EndDate = Request.EndDate;

            var Items = _unitOfWork.Items.Get().Where(x => x.Invoice.Date >= Request.StartDate && x.Invoice.Date <= Request.EndDate).ToList();
            result.GrandTotal = Math.Round(Items.Sum(x => x.Invoice.Total),2);
            double GrandTotal= Items.Sum(x => x.Invoice.Total);
     
            result.Categories = Items.GroupBy(x =>x.Product.Category.Name)
                              .Select(x => Factory.CreateCategory(x.Key,x.Sum(y => y.Invoice.SubTotal),GrandTotal)).ToList();
            return result;

        }
        
    }
}