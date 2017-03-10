using Billing.Database;
using Billing.Repository;
using System;
using System.Data;

namespace Billing.Seed
{
    public class Procurements
    {
        public static void Get()
        {
            IBillingRepository<Procurement> procurements = new BillingRepository<Procurement>(Help.Context);
            IBillingRepository<Supplier> suppliers = new BillingRepository<Supplier>(Help.Context);
            IBillingRepository<Product> products = new BillingRepository<Product>(Help.Context);
            DataTable rawData = Help.OpenExcel("Procurements");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                Supplier supplier = suppliers.Get(Help.dicSupp[Help.getInteger(row, 2)]);
                Product product = products.Get(Help.dicProd[Help.getInteger(row, 3)]);
                Procurement procurement = new Procurement()
                {
                    Document = Help.getString(row, 0),
                    Date = Help.getDate(row, 1),
                    Supplier = supplier,
                    Product = product,
                    Quantity = Help.getInteger(row, 4),
                    Price = Help.getDouble(row, 5)
                };
                N++;
                procurements.Insert(procurement);
            }
            procurements.Commit();
            Console.WriteLine(N);
        }
    }
}
