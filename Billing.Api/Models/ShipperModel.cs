using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class ShipperModel
    {
        public ShipperModel()
        {
            Invoices = new List<string>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<string> Invoices { get; set; }

    }
}