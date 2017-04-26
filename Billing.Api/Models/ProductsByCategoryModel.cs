using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class ProductsByCategoryModel
    {
        public ProductsByCategoryModel()
        {
            ProductsCategory = new List<ProductsByCategory>();
        }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductsByCategory> ProductsCategory { get; set; }
    }

    public class ProductsByCategory
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int Input { get; set; }
        public int Output { get; set; }
        public int Stock { get; set; }
    }
}