using Billing.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    public class SalesByRegionController : BaseController
    {
        public struct request
        {
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public int AgentId { get; set; }
        }

        public IHttpActionResult Post(request request)
        {
            SalesByRegionModel result = new SalesByRegionModel();

            result.StartDate = request.startDate;
            result.EndDate = request.endDate;
            //request.AgentId ????
            var Invoices = UnitOfWork.Invoices.Get().Where(x => x.Date >= request.startDate && x.Date <= request.endDate).ToList();
            result.GrandTotal = Invoices.Sum(x => x.Total);

            result.Sales = new List<RegionSalesModel>();
            var query = Invoices.GroupBy(x => x.Customer.Town.Region.ToString())
                                .Select(x => new
                                {
                                    RegionName = x.Key,
                                    RegionTotal = x.Sum(y => y.Total)
                                }).ToList();
            foreach (var item in query)
            {
                RegionSalesModel region = new RegionSalesModel()
                {
                    RegionName = item.RegionName,
                    RegionTotal = item.RegionTotal,
                    RegionPercent = 100 * item.RegionTotal / result.GrandTotal
                };
                region.Agents = new List<AgentSalesModel>();
                var agents = Invoices.Where(x => x.Customer.Town.Region.ToString() == item.RegionName)
                                    .GroupBy(x => new { id = x.Agent.Id, name = x.Agent.Name })
                                    .Select(x => new
                                    {
                                        AgentId = x.Key.id,
                                        AgentName = x.Key.name,
                                        AgentTotal = x.Sum(y => y.Total)
                                    }).ToList();
                foreach (var agent in agents)
                {
                    region.Agents.Add(new AgentSalesModel()
                    {
                        AgentId = agent.AgentId,
                        AgentName = agent.AgentName,
                        AgentTotal = agent.AgentTotal,
                        RegionPercent = 100 * agent.AgentTotal / region.RegionTotal,
                        TotalPercent = 100 * agent.AgentTotal / result.GrandTotal
                    });
                }
                result.Sales.Add(region);
            }

            return Ok(result);
        }
    }
}