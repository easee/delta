using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Api.Reports;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("user")]
    //dodati za own, da agent može vidjeti svoj
    public class DashboardController : BaseController
    {
        public IHttpActionResult Get(int id = 0)
        {
            try
            {
                return Ok(Reports.Dashboard.Report(id));
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
    }
}
