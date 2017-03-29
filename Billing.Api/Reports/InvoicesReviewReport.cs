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
    public class InvoicesReviewReport
    {
        private BillingIdentity identity = new BillingIdentity();
        private ReportFactory Factory = new ReportFactory();
        private UnitOfWork _unitOfWork;
        public InvoicesReviewReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public InvoicesReviewModel Report(RequestModel Request)
        {
            List<Invoice> Invoices = _unitOfWork.Invoices.Get()
                                    .Where(x => x.Date >= Request.StartDate && x.Date <= Request.EndDate/* && x.Customer.Id = Request.Id*/).ToList();
            InvoicesReviewModel result = new InvoicesReviewModel()
            {
                CustomerId = Request.Id,
                //CustomerName = 
                StartDate = Request.StartDate,
                EndDate = Request.EndDate,
                GrandTotal = Invoices.Sum(x => x.SubTotal)
            };
            return result;
        }
    }
}