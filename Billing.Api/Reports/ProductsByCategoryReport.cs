using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class ProductsByCategoryReport : BaseReport
    {

        public ProductsByCategoryReport(UnitOfWork unitOfWork) : base(unitOfWork) { }
        public ProductsByCategoryModel Report(int id)
        {
            Category Category = UnitOfWork.Categories.Get(id);
            if (Category == null) throw new Exception("Category not found");
            List<Product> Products = UnitOfWork.Products.Get().Where(x => x.Category.Id == id).ToList();
            ProductsByCategoryModel products = new ProductsByCategoryModel()
            {
                CategoryId = Category.Id,
                CategoryName = Category.Name
            };

            products.ProductsCategory = UnitOfWork.Products.Get().Where(x => x.Category.Id == id).ToList()
                                                             .OrderBy(x => x.Name)
                                                             .Select(x => Factory.Create(x.Name, x.Id, x.Stock))
                                                             .ToList();
            return products;
        }
    }
}