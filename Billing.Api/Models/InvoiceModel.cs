using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime Date { get; set; }
        public string Shipper { get; set; }
        public string Agent { get; set; }
        public string Customer { get; set; }
        public double Total { get; set; }
        public double Shipping { get; set; }
    }
}