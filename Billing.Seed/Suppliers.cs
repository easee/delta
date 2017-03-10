using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    public class Suppliers
    {
        public static void Get()
        {
            IBillingRepository<Supplier> suppliers = new BillingRepository<Supplier>(Help.Context);
            IBillingRepository<Town> towns = new BillingRepository<Town>(Help.Context);
            DataTable rawData = Help.OpenExcel("Suppliers");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = Help.getInteger(row, 0);
                string zip = Help.getString(row, 1);
                Town town = towns.Get().FirstOrDefault(x => x.Zip == zip);
                Supplier supp = new Supplier()
                {
                    Name = Help.getString(row, 2),
                    Address = Help.getString(row, 3),
                    Town = town
                };
                N++;
                suppliers.Insert(supp);
                suppliers.Commit();
                Help.dicSupp.Add(oldId, supp.Id);
            }
            Console.WriteLine(N);
        }
    }
}
