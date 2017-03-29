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
        private ProductsByCategoryReport _productsByCategory;
        private InvoiceReport _invoices;

        public DashboardReport Dashboard { get { return _dashboard ?? (_dashboard = new DashboardReport(_unitOfWork)); } }
        public SalesByRegionReport SalesByRegion { get { return _salesByRegion ?? (_salesByRegion = new SalesByRegionReport(_unitOfWork)); } }
        public SalesByCustomerReport SalesByCustomer { get { return _salesByCustomer ?? (_salesByCustomer = new SalesByCustomerReport(_unitOfWork)); } }
        public SalesByAgentReport SalesByAgent { get { return _salesByAgent ?? (_salesByAgent = new SalesByAgentReport(_unitOfWork)); } }
        public ProductsByCategoryReport ProductsByCategory { get { return _productsByCategory ?? (_productsByCategory = new ProductsByCategoryReport(_unitOfWork)); } }
        public InvoiceReport Invoice { get { return _invoices ?? (_invoices = new InvoiceReport(_unitOfWork)); } }
    }
}