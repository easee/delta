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

        public MonthlySales Create(Region region, double sales)
        {
            return new MonthlySales()
            {
                Label = region.ToString(),
                Sales = sales
            };
        }

        public List<AnnualSales> Create(List<InputItem> list, int Length = 12)
        {
            List<AnnualSales> result = new List<AnnualSales>();
            AnnualSales current = new AnnualSales(Length);
            foreach (var item in list)
            {
                if (item.Label != current.Label)
                {
                    if (current.Label != null) result.Add(current);
                    current = new AnnualSales(Length);
                    current.Label = item.Label;
                }
                current.Sales[item.Index - 1] = item.Value;
            }
            if (current.Label != null) result.Add(current);
            return result;
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

        public RegionSalesAgentModel Create(List<Invoice> InvoicesOfAgent, string Region, double Sales, double AgentTotal, List<Invoice> Invoices)
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
                RegionPercent = Math.Round(100 * Sales / total, 2),
                TotalPercent = Math.Round(100 * Sales / AgentTotal, 2)
            };
            return region;
        }

        public ProductsByCategory Create(string Name, int Id, int Input, int Output, int Stock)
        {
            ProductsByCategory products = new ProductsByCategory()
            {
                ProductId = Id,
                ProductName = Name,
                Input = Input,
                Output = Output,
                Stock = Stock
            };
            return products;
        }

        public List<CustomerStatus> Customers(List<InputItem> list)
        {
            List<CustomerStatus> result = new List<CustomerStatus>();
            CustomerStatus current = new CustomerStatus();
            foreach (var item in list)
            {
                if (item.Label != current.Name)
                {
                    if (current.Name != null) result.Add(current);
                    current = new CustomerStatus();
                    current.Name = item.Label;
                }
                current.Debit += item.Value;
                if (item.Index > 3) current.Credit += item.Value;
            }
            if (current.Name != null) result.Add(current);
            return result.OrderByDescending(x => x.Debit).ToList();
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
        public InvoiceReviewProducts Create(int Id, string Name, double Price, int Quantity, double SubTotal)
        {

            InvoiceReviewProducts products = new InvoiceReviewProducts()
            {
                ProductId = Id,
                ProductName = Name,
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

        public InvoiceInfoModel Create(int Id, string InvoiceNo, DateTime Date, DateTime ShippedOn, double Total, Status Status)
        {
            InvoiceInfoModel invoice = new InvoiceInfoModel();
            invoice.InvoiceId = Id;
            invoice.InvoiceNo = InvoiceNo;
            invoice.InvoiceDate = Date;
            invoice.ShippedOn = ShippedOn;
            invoice.InvoiceTotal = Total;
            invoice.InvoiceStatus = Status.ToString();

            return invoice;
        }
        public CustomerStatus Create(int Id, string Name, Status Status, double Amount)
        {
            return new CustomerStatus()
            {
                Id = Id,
                Name = Name,

            };
        }
    }
}