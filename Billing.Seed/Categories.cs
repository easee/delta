using Billing.Database;
using Billing.Repository;
using System;
using System.Data;

namespace Billing.Seed
{
    public class Categories
    {
        public static void Get()
        {
            IBillingRepository<Category> categories = new BillingRepository<Category>(Help.Context);
            DataTable rawData = Help.OpenExcel("Categories");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = Help.getInteger(row, 0);
                Category catt = new Category()
                {
                    Name = Help.getString(row, 1),
                };
                N++;
                categories.Insert(catt);
                categories.Commit();
                Help.dicCatt.Add(oldId, catt.Id);
            }
            Console.WriteLine(N);
        }
    }
}
