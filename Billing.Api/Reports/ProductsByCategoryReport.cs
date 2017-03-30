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
    public class ProductsByCategoryReport
    {

        private BillingIdentity identity = new BillingIdentity();
        private ReportFactory Factory = new ReportFactory();
        private UnitOfWork _unitOfWork;
        public ProductsByCategoryReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ProductsByCategoryModel Report(int id)
        {
            Category Category = _unitOfWork.Categories.Get(id);
            if (Category == null) throw new Exception("Category not found");
            List<Product> Products = _unitOfWork.Products.Get().Where(x => x.Category.Id == id).ToList();
            ProductsByCategoryModel products = new ProductsByCategoryModel()
            {
                CategoryId = Category.Id,
                CategoryName = Category.Name
            };

            products.ProductsCategory = _unitOfWork.Products.Get().Where(x => x.Category.Id == id).ToList()
                                                             .OrderBy(x => x.Name)
                                                             .Select(x => Factory.Create(x.Name, x.Id, x.Stock))
                                                             .ToList();
            return products;
        }
    }
}