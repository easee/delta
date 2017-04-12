using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    
    public class ProcurementModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Document { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        //IN GIGI's DOCUMENT 
        public double Total { get; set; }
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }

    }
}