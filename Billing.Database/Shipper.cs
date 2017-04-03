using System.Collections.Generic;

namespace Billing.Database
{
    public class Shipper : Partner
    {
        public Shipper()
        {
            Invoices = new List<Invoice>();
        }
        
        public virtual List<Invoice> Invoices { get; set; }
    }
}
