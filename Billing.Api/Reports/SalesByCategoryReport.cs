﻿using Billing.Api.Controllers;
using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Reports
{
    public class SalesByCategoryReport : BaseReport
    {
        public SalesByCategoryReport(UnitOfWork unitOfWork) : base(unitOfWork) { }

        public SalesByCategoryModel Report (RequestModel Request)
        {
            if (Request.EndDate < Request.StartDate) throw new Exception("Incorrect Date");
            SalesByCategoryModel result = new SalesByCategoryModel();

            result.StartDate = Request.StartDate;
            result.EndDate = Request.EndDate;

            var Category = UnitOfWork.Categories.Get().ToList();
            var Items = UnitOfWork.Items.Get().Where(x => x.Invoice.Date >= Request.StartDate && x.Invoice.Date <= Request.EndDate).ToList();
            result.GrandTotal = Math.Round(Items.Sum(x => x.SubTotal),2);
            double GrandTotal= Items.Sum(x => x.SubTotal);
     
            result.Categories = Items.OrderBy(x => x.Product.Category.Id) 
                                     .GroupBy(x =>  x.Product.Category.Name)
                                     .Select(x => Factory.Create(x.Key,x.Sum(y => y.SubTotal),GrandTotal,Category)).ToList();
            return result;

        }
        
    }
}