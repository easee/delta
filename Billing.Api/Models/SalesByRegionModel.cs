using System;
using System.Collections.Generic;

namespace Billing.Api.Models
{
    public class AgentSalesModel
    {
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public double AgentTotal { get; set; }
        public double RegionPercent { get; set; }
        public double TotalPercent { get; set; }
    }

    public class RegionSalesModel
    {
        public RegionSalesModel()
        {
            Agents = new List<AgentSalesModel>();
        }
        public string RegionName { get; set; }
        public double RegionTotal { get; set; }
        public double RegionPercent { get; set; }
        public List<AgentSalesModel> Agents { get; set; }
    }

    public class SalesByRegionModel
    {
        public SalesByRegionModel()
        {
            Sales = new List<RegionSalesModel>();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double GrandTotal { get; set; }
        public List<RegionSalesModel> Sales { get; set; }
    }
}