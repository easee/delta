using System.Collections.Generic;

namespace Billing.Database
{
    public class Category: Basic 
    {
        public Category()
        {
            Products = new List<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
