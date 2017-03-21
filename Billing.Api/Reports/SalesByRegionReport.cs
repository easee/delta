using Billing.Api.Controllers;
using Billing.Api.Models;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Billing.Api.Reports
{
    public class SalesByRegionReport
    {
        public static SalesByRegionModel Report(UnitOfWork UnitOfWork, RequestModel Request)
        {
            SalesByRegionModel result = new SalesByRegionModel();

            result.StartDate = Request.StartDate;
            result.EndDate = Request.EndDate;
            //request.Id ????

            var Invoices = UnitOfWork.Invoices.Get().Where(x => x.Date >= Request.StartDate && x.Date <= Request.EndDate).ToList();
            result.GrandTotal = Invoices.Sum(x => x.Total);

            result.Sales = new List<RegionSalesModel>();
            var query = Invoices.GroupBy(x => x.Customer.Town.Region.ToString())
                                .Select(x => new { RegionName = x.Key, RegionTotal = x.Sum(y => y.Total) })
                                .ToList();
            foreach (var item in query)
            {
                RegionSalesModel region = new RegionSalesModel()
                {
                    RegionName = item.RegionName,
                    RegionTotal = item.RegionTotal,
                    RegionPercent = Math.Round(100 * item.RegionTotal / result.GrandTotal, 2)
                };
                region.Agents = new List<AgentSalesModel>();
                var agents = Invoices.Where(x => x.Customer.Town.Region.ToString() == item.RegionName)
                                     .GroupBy(x => new { id = x.Agent.Id, name = x.Agent.Name })
                                     .Select(x => new { AgentId = x.Key.id, AgentName = x.Key.name, AgentTotal = x.Sum(y => y.Total) })
                                     .ToList();
                foreach (var agent in agents)
                {
                    region.Agents.Add(new AgentSalesModel()
                    {
                        AgentId = agent.AgentId,
                        AgentName = agent.AgentName,
                        AgentTotal = agent.AgentTotal,
                        RegionPercent = Math.Round(100 * agent.AgentTotal / region.RegionTotal, 2),
                        TotalPercent = Math.Round(100 * agent.AgentTotal / result.GrandTotal, 2)
                    });
                }
                result.Sales.Add(region);
            }

            return result;
        }
    }
}