using Billing.Api.Controllers;
using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class CrossAgentRegionReport : BaseReport
    {

        public CrossAgentRegionReport(UnitOfWork unitOfWork) : base(unitOfWork) { }
        public CrossAgentRegionReportModel Report(RequestModel Request)
        {
            if(Request.EndDate<Request.StartDate) throw new Exception("Incorrect Date");
            List<Invoice> Invoices = UnitOfWork.Invoices.Get().Where(x => x.Date >= Request.StartDate && x.Date <= Request.EndDate).ToList();
            double grandTotal = Invoices.Sum(x => x.SubTotal);

            //List<Invoice> AgentInvoices = Invoices.Where(x => x.Agent.Id == Request.Id).ToList();
            CrossAgentRegionReportModel result = new CrossAgentRegionReportModel()
            {
                StartDate = Request.StartDate,
                EndDate = Request.EndDate,
                GrandTotal = grandTotal
            };
            
       
            result.Regions = Invoices.GroupBy(x => x.Customer.Town.Region.ToString())
                            .Select(x =>Factory.CreateRegion(x.Sum(y => y.SubTotal),x.Key)).ToList();
            int number = result.Regions.Count;
            result.Agents = Invoices.GroupBy(x => x.Agent.Name)
                             .Select(x => Factory.CreateAgent(x.Sum(y => y.SubTotal),x.Key,Invoices,number,result.Regions,Request)).ToList();      
            return result;
        }
    }
}