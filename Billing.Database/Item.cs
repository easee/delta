using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Database
{
    public class Item : Basic
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public double SubTotal { get { return (Quantity * Price); } }

        [Required]
        public virtual Invoice Invoice { get; set; }
        [Required]
        public virtual Product Product { get; set; }
    }
}
