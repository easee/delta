using Billing.Database;
using Billing.Repository;
using System;
using System.Data;

namespace Billing.Seed
{
    public class Products
    {
        public static void Get()
        {
            IBillingRepository<Product> products = new BillingRepository<Product>(Help.Context);
            IBillingRepository<Stock> stocks = new BillingRepository<Stock>(Help.Context);
            IBillingRepository<Category> categories = new BillingRepository<Category>(Help.Context);

            DataTable rawData = Help.OpenExcel("Products");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = Help.getInteger(row, 0);
                Product prod = new Product()
                {
                    Name = Help.getString(row, 1),
                    Unit = Help.getString(row, 2),
                    Price = Help.getDouble(row, 4),
                    Category = categories.Get(Help.dicCatt[Help.getInteger(row, 3)])
                };
                N++;
                products.Insert(prod);
                products.Commit();
                stocks.Insert(new Stock() { Id = prod.Id });
                products.Commit();
                Help.dicProd.Add(oldId, prod.Id);
            }
            Console.WriteLine(N);
        }
    }
}