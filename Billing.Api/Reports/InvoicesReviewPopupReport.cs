using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class InvoicesReviewPopupReport
    {

        private BillingIdentity identity = new BillingIdentity();
        private ReportFactory Factory = new ReportFactory();
        private UnitOfWork _unitOfWork;
        public InvoicesReviewPopupReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public InvoiceReviewPopupModel Report(int id)
        {
            Invoice Invoice = _unitOfWork.Invoices.Get(id);
            InvoiceReviewPopupModel result = new InvoiceReviewPopupModel
            {
                InvoiceNo = Invoice.InvoiceNo,
                CustomerName = Invoice.Customer.Name,
                InvoiceDate = Invoice.Date,
                InvoiceStatus = Invoice.Status.ToString(),
                Subtotal = Invoice.SubTotal,
                VatAmount = Invoice.VatAmount,
                Shipper = Invoice.Shipper.Name,
                Shipping = Invoice.Shipping,
                ShippedOn = Invoice.ShippedOn.Value
            };

            result.Products = _unitOfWork.Items.Get().Where(x => x.Invoice.Id == id).ToList()
                                        .Select(x => Factory.Create(x.Product.Id, x.Product.Name, x.Price, x.Quantity, x.SubTotal))
                                        .ToList();
            return result;
        }
    }
}