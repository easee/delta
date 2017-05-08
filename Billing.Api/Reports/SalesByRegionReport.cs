using Billing.Api.Controllers;
using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Billing.Api.Reports
{
    public class SalesByRegionReport : BaseReport
    {
        public SalesByRegionReport(UnitOfWork unitOfWork) : base(unitOfWork) { }

        public SalesByRegionModel Report(RequestModel Request)
        {
            if (Request.EndDate < Request.StartDate) throw new Exception("Incorrect Date");

            List<Invoice> Invoices = UnitOfWork.Invoices.Get().Where(x => x.Date >= Request.StartDate && x.Date <= Request.EndDate).ToList();
            SalesByRegionModel result = new SalesByRegionModel()
            {
                StartDate = Request.StartDate,
                EndDate = Request.EndDate,
                GrandTotal = Invoices.Sum(x => x.SubTotal)
            };

            result.Sales = Invoices.OrderBy(x => x.Customer.Id).ToList()
                                   .GroupBy(x => x.Customer.Town.Region.ToString())
                                   .Select(x => Factory.Create
                                   (Invoices, x.Key, x.Sum(y => y.SubTotal)))
                                   .ToList();
            return result;
        }
    }
}