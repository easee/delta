using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    public class Customers
    {
        public static void Get()
        {
            IBillingRepository<Customer> customers = new BillingRepository<Customer>(Help.Context);
            IBillingRepository<Town> towns = new BillingRepository<Town>(Help.Context);
            DataTable rawData = Help.OpenExcel("Customers");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = Help.getInteger(row, 0);
                string zip = Help.getString(row, 1);
                Town town = towns.Get().FirstOrDefault(x => x.Zip == zip);
                Customer cust = new Customer()
                {
                    Name = Help.getString(row, 2),
                    Address = Help.getString(row, 3),
                    Town = town
                };
                N++;
                customers.Insert(cust);
                customers.Commit();
                Help.dicCust.Add(oldId, cust.Id);
            }
            Console.WriteLine(N);
        }
    }
}
