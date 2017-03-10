using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    public class Items
    {
        public static void Get()
        {
            IBillingRepository<Item> items = new BillingRepository<Item>(Help.Context);
            IBillingRepository<Invoice> invoices = new BillingRepository<Invoice>(Help.Context);
            IBillingRepository<Product> products = new BillingRepository<Product>(Help.Context);
            DataTable rawData = Help.OpenExcel("Items");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                string invoiceNo = Help.getString(row, 0);
                Invoice invoice = invoices.Get().FirstOrDefault(x => x.InvoiceNo == invoiceNo);
                Product product = products.Get(Help.dicProd[Help.getInteger(row, 1)]);
                Item item = new Item()
                {
                    Invoice = invoice,
                    Product = product,
                    Quantity = Help.getInteger(row, 2),
                    Price = Help.getDouble(row, 3)
                };
                N++;
                items.Insert(item);
            }
            items.Commit();
            Console.WriteLine(N);
        }
    }
}
