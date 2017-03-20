using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public virtual Town Town { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}
