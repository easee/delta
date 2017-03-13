﻿using System.Collections.Generic;

namespace Billing.Database
{
    public class Product : Basic
    {
        public Product()
        {
            Items = new List<Item>();
            Procurements = new List<Procurement>();
            
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }

        public virtual Category Category { get; set; }
        public virtual Stock Stock { get; set; }
        public virtual List<Procurement> Procurements { get; set; }
        public virtual List<Item> Items { get; set; }

        // ovo je komentar
    }
}
