using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class CrossAgentRegionReportModel
    {
        public CrossAgentRegionReportModel()
        {
            Regions = new List<CrossRegion>();
            Agents = new List<CrossAgent>();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double GrandTotal { get; set; }
        public List<CrossRegion> Regions { get; set; }
        public List<CrossAgent> Agents { get; set; }
    }
    public class CrossAgent
    {
        public CrossAgent(int lenght)
        {
            RegionSales = new double[lenght];

        }
        public string AgentName { get; set; }
        public double AgentTurnover { get; set; }
        public double[] RegionSales { get; set; }
    }

    public class CrossRegion
    {
        public string RegionName { get; set; }
        public double RegionTotal { get; set; }
    }
}