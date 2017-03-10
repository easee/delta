using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Database
{
    public class Invoice : Basic
    {
        public Invoice()
        {
            Items = new List<Item>();
        }

        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime ShippedOn { get; set; }
        public int Status { get; set; }
        [NotMapped]
        public double SubTotal
        {
            get
            {
                double sum = 0;
                foreach (Item item in Items) sum += item.SubTotal;
                return sum;
            }
        }
        public double Vat { get; set; }
        [NotMapped]
        public double VatAmount { get { return (SubTotal * Vat / 100); } }
        public double Shipping { get; set; }
        [NotMapped]
        public double Total { get { return (SubTotal + VatAmount + Shipping); } }

        public virtual Agent Agent { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Shipper Shipper { get; set; }
        public virtual List<Item> Items { get; set; }
    }
}
