using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Helpers
{
    public class InvoiceHelper
    {
        Invoice Invoice = new Invoice();

        public Invoice NextStep(UnitOfWork unitOfWork, int id, bool Cancel)
        {
            Invoice = unitOfWork.Invoices.Get(id);
            if (Invoice != null)
            {
                switch (Invoice.Status) //Pogledamo na osnovu Invoice statusa koju ćemo metodu pozvati
                {
                    case Status.OrderCreated: OrderCreated(Cancel); break;//Ako je kreirana faktura zovemo tu metodu
                    case Status.InvoiceCreated: InvoiceCreated(Cancel); break;
                    case Status.InvoiceSent: InvoiceSent(Cancel); break;
                    case Status.InvoicePaid: InvoicePaid(); break;
                    case Status.InvoiceOnHold: InvoiceOnHold(); break;
                    case Status.InvoiceReady: InvoiceReady(); break;
                        //default:
                }
                unitOfWork.Invoices.Update(Invoice, id);
                //U History upisujemo novi događaj od te fakture
                unitOfWork.History.Insert(new Event() { Invoice = Invoice, Date = DateTime.Today, Status = Invoice.Status });
                unitOfWork.Commit();
            }
            return Invoice;
        }
        //Ukoliko smo dobili instrukciju da poništimo narudžbu, tada će status biti Canceled. 
        private void OrderCreated(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = Status.Canceled;
            }
            else
            {
                Invoice.Status = Status.InvoiceCreated;
            }
        }

        private void InvoiceCreated(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = Status.Canceled;
            }
            else
            {
                Invoice.Status = Status.InvoiceSent;
            }
        }
        //Ovdje ispod idemo iz statusa InvoiceSent u InvoicePaid, ako se zadovolji uslov da nije Cancel.
        private void InvoiceSent(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = Status.Canceled;
            }
            else
            {
                Invoice.Status = Status.InvoicePaid;
            }
        }

        private void InvoicePaid()
        {
            Invoice.Status = Status.InvoiceReady;//Postavljamo status na InvoiceReady
            foreach (var Item in Invoice.Items)
            {
                if (Item.Product.Stock.Inventory < Item.Quantity)
                {
                    Invoice.Status = Status.InvoiceOnHold; //Prolazeći kroz sve iteme.
                    break; //Prekidamo petlju čim naiđemo na vrijednost koja ne zadovoljava uslov.
                }
            }
        }

        private void InvoiceOnHold()
        {
            Invoice.Status = Status.InvoiceReady; 
            foreach (var Item in Invoice.Items)
            {
                if (Item.Product.Stock.Inventory < Item.Quantity)
                {
                    Invoice.Status = Status.InvoiceOnHold;
                    break;
                }
            }
        }

        private void InvoiceReady()
        {
            Invoice.Status = Status.InvoiceShipped;
            Invoice.ShippedOn = DateTime.Today;
        }
    }
}