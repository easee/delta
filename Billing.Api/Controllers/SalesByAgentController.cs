using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Api.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("user")]
    public class SalesByAgentController : BaseController
    {
        public IHttpActionResult Post(RequestModel request)
        {

            try
            {
                return Ok(Reports.SalesByAgent.Report(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}