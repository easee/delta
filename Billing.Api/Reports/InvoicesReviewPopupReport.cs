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
    public class InvoicesReviewPopupReport : BaseReport
    {

        public InvoicesReviewPopupReport(UnitOfWork unitOfWork) : base(unitOfWork) { }
        public InvoiceReviewPopupModel Report(int id)
        {
            Invoice Invoice = UnitOfWork.Invoices.Get(id);
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

            result.Products = UnitOfWork.Items.Get().Where(x => x.Invoice.Id == id).ToList()
                                        .Select(x => Factory.Create(x.Product.Id, x.Product.Name, x.Price, x.Quantity, x.SubTotal))
                                        .ToList();
            return result;
        }
    }
}