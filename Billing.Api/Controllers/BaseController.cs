using Billing.Api.Models;
using Billing.Api.Reports;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    public class BaseController : ApiController
    {
        private UnitOfWork _unitOfWork;
        private Factory _factory;
        private SetOfReports _reports;

        protected UnitOfWork UnitOfWork { get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork()); } }

        protected Factory Factory { get { return _factory ?? (_factory = new Factory(UnitOfWork)); } }

        protected SetOfReports Reports { get { return _reports ?? (_reports = new SetOfReports(UnitOfWork)); } }
    }
}