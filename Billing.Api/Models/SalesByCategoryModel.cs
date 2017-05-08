using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class CategoriesSalesModel {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double CategoryTotal { get; set; }
        public double CategoryPercent { get; set; }
    }
    public class SalesByCategoryModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double GrandTotal { get; set; }

        public List<CategoriesSalesModel> Categories { get; set; }
    }
}