using Billing.Api.Models;
using Billing.Database;
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
                AgentTotal = x.Sum(y => y.Total),
                RegionPercent = 100 * x.Sum(y => y.Total) / Sales,
                TotalPercent = 100 * x.Sum(y => y.Total) / GrandTotal
            });
            region.Agents = q3.ToList();
            return region;
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
                               .Select(x => new { RegionName = x.Key, RegionTotal = x.Sum(y => y.Total) })
                               .ToList();
            double total = 0;
            foreach (var item in query)
                if (item.RegionName.Equals(Region))
                    total = item.RegionTotal;

            RegionSalesAgentModel region = new RegionSalesAgentModel()
            {
                RegionName = Region,
                RegionTotal = Math.Round(Sales,2),
                RegionPercent = Math.Round(100 * Sales / total, 2),
                TotalPercent = Math.Round(100 * Sales / AgentTotal, 2)
            };
            return region;
        }

        public ProductsByCategory Create(string Name,int Id,int Input,int Output,int Stock)
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

        public InvoiceProductReport Create(int Id, string Name, string Unit,double Price,int Quantity, double SubTotal)
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