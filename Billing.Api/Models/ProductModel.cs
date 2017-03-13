using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class ProductModel
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Unit { get; set; }
        //public double Price { get; set; }
        //public string Category { get; set; }
        //public int Stock { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public double Input { get; set; }
        public double Output { get; set; }
        public double Inventory { get; set; }
    }
}