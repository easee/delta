using Billing.Api.Models;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    public class DashboardController : BaseController
    {
        public IHttpActionResult Get() {

            DashboardModel result = new DashboardModel(8);
            return Ok(result);
        }


    }
}
