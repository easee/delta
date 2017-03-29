﻿using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class InvoicesReport
    {

        private BillingIdentity identity = new BillingIdentity();
        private ReportFactory Factory = new ReportFactory();
        private UnitOfWork _unitOfWork;
        public InvoicesReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public InvoiceReportModel Report(int id)
        {
            Invoice Invoice = _unitOfWork.Invoices.Get(id);
            if (Invoice == null) throw new Exception("Invoice not found");
                      
            InvoiceReportModel result = new InvoiceReportModel()
            {
                InvoiceNo = Invoice.InvoiceNo,
                InvoiceDate = Invoice.Date,
                CustomerId = Invoice.Customer.Id,
                CustomerName = Invoice.Customer.Name,
                CustomerAddress = Invoice.Customer.Address,
                ZipCode = Invoice.Customer.Town.Zip,
                Town = Invoice.Customer.Town.Name,
                Salesperson = Invoice.Agent.Name,
                ShippedDate = Invoice.ShippedOn.Value,
                ShippedVia = Invoice.Shipper.Name,
                InvoiceSubtotal = Invoice.SubTotal,
                VatAmount = Invoice.VatAmount,
                Shipping = Invoice.Shipping,
                InvoiceTotal = Invoice.Total
            };

            foreach (var item in Invoice.History)
            {
                DateTime Date;
                if (item.Status == 0)
                {
                    Date = item.Date;
                    result.OrderDate = Date;
                    break;
                }
            }


            result.Products = _unitOfWork.Items.Get().Where(x => x.Invoice.Id == id).ToList()
                                        .Select(x => Factory.Create( x.Product.Id, x.Product.Name,x.Product.Unit,x.Price,x.Quantity,x.SubTotal))
                                        .ToList();
            
            return result;
        }
    }
}