using Billing.Api.Controllers;
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

        public CategoryPurchaseModel Create(string Name,double SubTotal)
        {
            CategoryPurchaseModel category = new CategoryPurchaseModel()
            {
                CategoryName = Name,
                CategoryTotal = SubTotal
            };
            return category;
        }
        public CustomerPurchaseModel Create(string Name,double SubTotal,List<Item> Items,int number,List<CategoryPurchaseModel> Catquery,RequestModel Request)
        {
            CustomerPurchaseModel customer = new CustomerPurchaseModel(number)
            {
                CustomerName=Name,
                CustomerTurnover=SubTotal
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