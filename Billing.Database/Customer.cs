using System.Collections.Generic;

namespace Billing.Database
{
    public class Customer : Basic
    {
        public Customer()
        {
            Invoices = new List<Invoice>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual Town Town { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}
