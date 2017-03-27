using Billing.Api.Controllers;
using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using Billing.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class SalesByAgentReport
    {
        private BillingIdentity identity = new BillingIdentity();
        private ReportFactory Factory = new ReportFactory();
        private UnitOfWork _unitOfWork;
        public SalesByAgentReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public SalesByAgentModel Report(RequestModel Request)
        {
            List<Invoice> Invoices = _unitOfWork.Invoices.Get().Where(x => x.Date >= Request.StartDate && x.Date <= Request.EndDate).ToList();
            List<Invoice> InvoicesOfAgent = _unitOfWork.Invoices.Get().Where(x => x.Date >= Request.StartDate && x.Date <= Request.EndDate && x.Agent.Id == Request.Id).ToList();
            double grandTotal = Math.Round(Invoices.Sum(x => x.Total), 2);
            Agent agent = _unitOfWork.Agents.Get(Request.Id);
            double AgentTotal = Math.Round(InvoicesOfAgent.Sum(x => x.Total), 2);
            SalesByAgentModel result = new SalesByAgentModel()
            {
                AgentName = agent.Name,
                StartDate = Request.StartDate,
                EndDate = Request.EndDate,
                AgentTotal = AgentTotal,
                PercentTotal = Math.Round(100 *InvoicesOfAgent.Sum(x => x.Total) / grandTotal, 2)               
            };
           
            result.Sales = InvoicesOfAgent.OrderBy(x => x.Customer.Id).ToList()
                                          .GroupBy(x => x.Customer.Town.Region.ToString())
                                          .Select(x => Factory.Create
                                          (InvoicesOfAgent,x.Key, x.Sum(y => y.Total),AgentTotal,Invoices))
                                          .ToList();
           
            return result;
        }
    }
}