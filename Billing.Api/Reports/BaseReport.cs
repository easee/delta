using Billing.Api.Helpers;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class BaseReport
    {
        private UnitOfWork _unitOfWork;
        private ReportFactory _factory;
        private BillingIdentity _identity;

        public BaseReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected BillingIdentity Identity
        {
            get { return _identity ?? (_identity = new BillingIdentity(UnitOfWork)); }
        }

        protected ReportFactory Factory
        {
            get { return _factory ?? (_factory = new ReportFactory()); }
        }

        protected UnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork()); }
        }
    }
}