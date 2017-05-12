using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class TownModel
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Zip { get; set; }
        public string Region { get; set; }
        public int RegionId { get; set; }
    }
}

 