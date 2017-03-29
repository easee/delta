using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Repository;
using System;
using System.Web.Http;

namespace Billing.Api.Reports
{
    public class SetOfReports
    {
        private UnitOfWork _unitOfWork;

        public SetOfReports(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private DashboardReport _dashboard;
        private SalesByRegionReport _salesByRegion;
        private SalesByCustomerReport _salesByCustomer;
        private SalesByAgentReport _salesByAgent;
        private CustomersByCategoryReport _customersByCategory;
        private CrossAgentRegionReport _crossAgentRegion;
        private SalesByCategoryReport _salesByCategory;
        private InvoicesReviewReport _invoiceReview;
        public DashboardReport Dashboard { get { return _dashboard ?? (_dashboard = new DashboardReport(_unitOfWork)); } }
        public SalesByRegionReport SalesByRegion { get { return _salesByRegion ?? (_salesByRegion = new SalesByRegionReport(_unitOfWork)); } }
        public SalesByCustomerReport SalesByCustomer { get { return _salesByCustomer ?? (_salesByCustomer = new SalesByCustomerReport(_unitOfWork)); } }
        public SalesByAgentReport SalesByAgent { get { return _salesByAgent ?? (_salesByAgent = new SalesByAgentReport(_unitOfWork)); } }
        public CustomersByCategoryReport CustomersByCategory { get { return _customersByCategory ?? (_customersByCategory = new CustomersByCategoryReport(_unitOfWork)); } }
        public CrossAgentRegionReport CrossAgentRegion { get { return _crossAgentRegion ?? (_crossAgentRegion = new CrossAgentRegionReport(_unitOfWork)); } }
        public SalesByCategoryReport SalesByCategory { get { return _salesByCategory ?? (_salesByCategory = new SalesByCategoryReport(_unitOfWork)); } }



        public InvoicesReviewReport InvoiceReview { get { return _invoiceReview ?? (_invoiceReview = new InvoicesReviewReport(_unitOfWork)); } }

    }
}