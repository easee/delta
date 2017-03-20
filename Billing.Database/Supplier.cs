using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Billing.Database
{
    public class Supplier : Basic
    {
        public Supplier()
        {
            Procurements = new List<Procurement>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Town_Id { get; set; }

        [Required]
        public virtual Town Town { get; set; }
        public virtual List<Procurement> Procurements { get; set; }
    }
}
