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
    public class ReportFactory
    {

        public List<DashboardModel.Customer> Create(List<InputItem> list)
        {
            List<DashboardModel.Customer> result = new List<DashboardModel.Customer>();
            DashboardModel.Customer current = new DashboardModel.Customer { Name = "", Credit = 0, Debit = 0 };
            foreach (var item in list)
            {
                if (item.Label != current.Name)
                {
                    if (current.Name != "") result.Add(current);
                    current = new DashboardModel.Customer { Name = item.Label, Credit = 0, Debit = 0 };
                }
                current.Debit += Math.Round(item.Value, 2);
                if (item.Index == 1) current.Credit += Math.Round(item.Value, 2);
            }
            if (current.Name != "") result.Add(current);
            return result.OrderByDescending(x => x.Debit).Take(10).ToList();
        }

        public List<DashboardModel.Burning> Create(List<BurningItem> burning)
        {
            List<DashboardModel.Burning> result = new List<DashboardModel.Burning>();

            DashboardModel.Burning current = new DashboardModel.Burning { Name = "", Ordered = 0, Stock = 0, Sold = 0 };
            foreach (var item in burning)
            {
                if (item.Product != current.Name)
                {
                    if (current.Name != "") if (current.Ordered > current.Stock || current.Stock < 0) result.Add(current);
                    current = new DashboardModel.Burning { Name = item.Product, Ordered = 0, Stock = item.Stock, Sold = 0 };
                }
                if (item.Status < Status.InvoicePaid) current.Ordered += item.Quantity; else current.Sold += item.Quantity;
            }
            if (current.Name != "") if (current.Ordered > current.Stock || current.Stock < 0) result.Add(current);

            return result.OrderByDescending(x => x.Difference).ToList();
        }


        public RegionSalesModel Create(List<Invoice> Invoices, string Region, double Sales)
        {
            double GrandTotal = Invoices.Sum(x => x.SubTotal);
            RegionSalesModel region = new RegionSalesModel()
            {
                RegionName = Region,
                RegionTotal = Sales,
                RegionPercent = Math.Round(100 * Sales / GrandTotal, 2)
            };
            var q1 = Invoices.Where(x => x.Customer.Town.Region.ToString() == Region);
            var q2 = q1.GroupBy(x => new { id = x.Agent.Id, name = x.Agent.Name });
            var q3 = q2.Select(x => new AgentSalesModel()
            {
                AgentId = x.Key.id,
                AgentName = x.Key.name,
                AgentTotal = x.Sum(y => y.SubTotal),
                RegionPercent = Math.Round(100 * x.Sum(y => y.SubTotal) / Sales,2),
                TotalPercent = Math.Round(100 * x.Sum(y => y.SubTotal) / GrandTotal,2)
            });
            region.Agents = q3.ToList();
            return region;
        }
        public CrossRegion CreateRegion(double SubTotal, string Name)
        {
            CrossRegion region = new CrossRegion()
            {
                RegionName = Name,
                RegionTotal = SubTotal
            };
            return region;
        }

        public CrossAgent CreateAgent(double SubTotal, string Name, List<Invoice> Invoices, int number, List<CrossRegion> Regions, RequestModel Request)
        {
            CrossAgent agent = new CrossAgent(number)
            {
                AgentName = Name,
                AgentTurnover = SubTotal
            };
            int i = 0;
            foreach (var reg in Regions)
            {
                var query = Invoices.Where
                    (x => x.Agent.Name.Equals(agent.AgentName) && x.Customer.Town.Region.ToString().Equals(reg.RegionName) && x.Date >= Request.StartDate && x.Date <= Request.EndDate).ToList();
                agent.RegionSales[i] = query.Sum(x => x.SubTotal);
                i++;
            }
            return agent;
        }
        public CustomerSalesModel Create(List<Invoice> Invoices, double Sales, string Name)
        {

            double GrandTotal = Invoices.Sum(x => x.SubTotal);
            CustomerSalesModel customer = new CustomerSalesModel()
            {
                CustomerName = Name,
                CustomerTurnover = Sales,
                CustomerPercent = Math.Round(100 * Sales / GrandTotal, 2)
            };

            return customer;
        }

        public RegionSalesAgentModel Create(List<Invoice> InvoicesOfAgent, string Region, double Sales, double AgentTotal, List<Invoice> Invoices,double grandTotal)
        {
            var query = Invoices.GroupBy(x => x.Customer.Town.Region.ToString())
                               .Select(x => new { RegionName = x.Key, RegionTotal = x.Sum(y => y.SubTotal) })
                               .ToList();
            double total = 0;
            foreach (var item in query)
                if (item.RegionName.Equals(Region))
                    total = item.RegionTotal;

            RegionSalesAgentModel region = new RegionSalesAgentModel()
            {
                RegionName = Region,
                RegionTotal = Math.Round(Sales, 2),
                RegionPercent = Math.Round(100 * Sales / AgentTotal, 2),
                TotalPercent = Math.Round(100 * Sales / grandTotal, 2)
            };
            return region;
        }

        public ProductsByCategory Create(string Name, int Id, Stock Stock,string Unit)
        {

            ProductsByCategory products = new ProductsByCategory()
            {
                ProductId = Id,
                ProductName = Name,
                Unit = Unit,
                Input = (Stock!=null) ? (int) Stock.Input : 0,
                Output = (Stock != null) ? (int)Stock.Output : 0,
                Stock = (Stock != null) ? (int)Stock.Id : 0,
            };
            return products;
        }

       

        public InvoiceProductReport Create(int Id, string Name, string Unit, double Price, int Quantity, double SubTotal)
        {

            InvoiceProductReport products = new InvoiceProductReport()
            {
                ProductId = Id,
                ProductName = Name,
                ProductUnit = Unit,
                Price = Price,
                Quantity = Quantity,
                Subtotal = SubTotal
            };
            return products;
        }
        public InvoiceReviewProducts Create(int Id, string Name, double Price, int Quantity, double SubTotal,string Unit)
        {

            InvoiceReviewProducts products = new InvoiceReviewProducts()
            {
                ProductId = Id,
                ProductName = Name,
                Unit=Unit,
                Price = Price,
                Quantity = Quantity,
                Subtotal = SubTotal
            };
            return products;
        }

        public CategoriesSalesModel CreateCategory(string Name, double SubTotal, double grandTotal)


        {
            CategoriesSalesModel category = new CategoriesSalesModel()
            {

                CategoryName = Name,
                CategoryTotal = Math.Round(SubTotal, 2),
                CategoryPercent = Math.Round(100 * SubTotal / grandTotal, 2)
            };
            return category;
        }

        public CategoryPurchaseModel Create(string Name, double SubTotal)
        {
            CategoryPurchaseModel category = new CategoryPurchaseModel()
            {
                CategoryName = Name,
                CategoryTotal = SubTotal
            };
            return category;
        }

        public SalesByProduct Create(string Name,double Price, double Percent, double TotalPercent)
        {
            SalesByProduct product = new SalesByProduct()
            {
                ProductName = Name,
                ProductTotal = Price,
                ProductPercent = Math.Round(100 * Price / Percent, 2),
                TotalPercent = Math.Round(100*Price/TotalPercent,2)
            };
            return product;
        }
        
        public CustomerPurchaseModel Create(string Name, double SubTotal, List<Item> Items, int number, List<CategoryPurchaseModel> Catquery, RequestModel Request)

        {
            CustomerPurchaseModel customer = new CustomerPurchaseModel(number)
            {
                CustomerName = Name,
                CustomerTurnover = SubTotal
            };
            int i = 0;
            foreach (var cat in Catquery)
            {
                var query = Items.Where(x => x.Invoice.Customer.Name.Equals(customer.CustomerName) && x.Product.Category.Name.Equals(cat.CategoryName) && x.Invoice.Date >= Request.StartDate && x.Invoice.Date <= Request.EndDate).ToList();
                customer.CategorySales[i] = query.Sum(x => x.SubTotal);
                i++;
            }

            return customer;
        }

        public InvoiceInfoModel Create(int Id, string InvoiceNo, DateTime Date, DateTime? ShippedOn, double Total, Status Status)
        {

            InvoiceInfoModel invoice = new InvoiceInfoModel();
            invoice.InvoiceId = Id;
            invoice.InvoiceNo = InvoiceNo;
            invoice.InvoiceDate = Date;
            invoice.ShippedOn = (ShippedOn !=null) ? ShippedOn.Value : DateTime.Now;
            invoice.InvoiceTotal = Math.Round(Total,2);
            invoice.InvoiceStatus = Status.ToString();

            return invoice;
        }
      
    }
}