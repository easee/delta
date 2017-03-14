using Billing.Database;

namespace Billing.Tests
{
    static public class TestHelper
    {
        static public void InitDatabase()
        {
            using (BillingContext context = new BillingContext())
            {
                context.Database.Delete();
                context.Database.Create();

                Category category = context.Categories.Add(new Category() { Name = "Test category number one" });
                context.SaveChanges();

                Product product1 = context.Products.Add(new Product() { Name = "Test product number one", Unit = "pcs", Price = 100, Category = category });
                Product product2 = context.Products.Add(new Product() { Name = "Test product number two", Unit = "pcs", Price = 100, Category = category });
                Product product3 = context.Products.Add(new Product() { Name = "Test product number three", Unit = "pcs", Price = 100, Category = category });
                Product product4 = context.Products.Add(new Product() { Name = "Test product number four", Unit = "pcs", Price = 100, Category = category });
                Product product5 = context.Products.Add(new Product() { Name = "Test product number five", Unit = "pcs", Price = 100, Category = category });
                context.SaveChanges();


                context.Stocks.Add(new Stock() { Id = product1.Id, Input = 120, Output = 66 });
                context.Stocks.Add(new Stock() { Id = product2.Id, Input = 120, Output = 66 });
                context.Stocks.Add(new Stock() { Id = product3.Id, Input = 120, Output = 66 });
                context.Stocks.Add(new Stock() { Id = product4.Id, Input = 120, Output = 66 });
                context.Categories.Add(new Category() { Name = "Test category number two" });
                context.SaveChanges();
            }
        }
    }
}