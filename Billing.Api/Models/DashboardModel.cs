﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{

    public class BurningItemModel {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Stock { get; set; }
        public int Ordered { get; set; }
        public int Sold { get; set; }
    }

    public class CustomerStatusModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
    }
    public class SalesMonthly
    {
        public string Label { get; set; }
        public double Sales { get; set; }
    }

    public class SalesCurrentYear {
        public SalesCurrentYear(){

                Sales = new double[12];
            }

        public string Label { get; set; }
        //MOze biti nesto po mjesecu.. bilo sta LAPTOP, REGIJA
        public double[] Sales { get; set; }
    }

    public class EmployeesSales {
        public EmployeesSales(int length)
        {
            Sales = new double[length];
        }


        public string Employee { get; set; }
        public double[] Sales { get; set; }//po regijama 

    }


    public class DashboardModel
    {

        public DashboardModel(int StatusCount)
        {
            CurrentMonth = new List<Models.SalesMonthly>();
            RegionsCurrentYear = new List<Models.SalesCurrentYear>();
            CategoriesCurrentYear = new List<Models.SalesCurrentYear>();
            EmployeesSales = new List<Models.EmployeesSales>();
            Top5Products = new List<SalesMonthly>();
            Invoices = new int[StatusCount];
            BurningItems = new List<BurningItemModel>();
            Customers = new List<CustomerStatusModel>();
        }

        public List<SalesMonthly> CurrentMonth { get; set; }
        public List<SalesCurrentYear> RegionsCurrentYear { get; set; }
        public List<SalesCurrentYear> CategoriesCurrentYear { get; set; }
        public List<EmployeesSales> EmployeesSales { get; set; }
        public List<SalesMonthly> Top5Products { get; set; }
        public int[] Invoices { get; set; }
        public List<BurningItemModel> BurningItems { get; set; }
        public List<CustomerStatusModel> Customers { get; set; }
    }
}