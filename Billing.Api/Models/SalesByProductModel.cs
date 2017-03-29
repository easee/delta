using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class SalesByProduct{
        public string ProductName { get; set; }
        public double ProductTotal { get; set; }
        public double ProductPercent { get; set; }
        public double TotalPercent { get; set; }
    }
    public class SalesByProductModel
    {
        public SalesByProductModel()
        {
            Products = new List<SalesByProduct>();
        }

        public string CategoryName  { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double CategoryTotal { get; set; }
        public double PercentTotal { get; set; }
        public List<SalesByProduct> Products { get; set; }
    }
}