using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.Api.Controllers;
using System.Threading;
using Billing.Api.Models;
using System.Web.Http.Routing;
using System.Web.Http.Hosting;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Collections.Generic;

namespace Billing.Tests
{
    [TestClass]
    public class TestSuppliersController
    {
        SuppliersController controller = new SuppliersController();
        HttpConfiguration config = new HttpConfiguration();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/suppliers");

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "suppliers" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllSuppliers()
        {
            TestHelper.InitDatabase(); GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod] //PASSED
        public void GetSupplierById()
        {
            GetReady();
            var actRes = controller.Get(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod] //PASSED
        public void GetSupplierByWrongId()
        {
            GetReady();
            var actRes = controller.Get(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod] //PASSED
        public void GetSupplierByName()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/name/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "suppliers" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            var actRes = controller.Get("Dell");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }


        [TestMethod] //PASSED
        public void PostSupplierGood()
        {
            GetReady();
            var actRes = controller.Post(new SupplierModel() { Name = "Dobavljac", Address = "Junacka 1", TownId = 1});
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod] //PASSED
        public void ChangeSupplierName()
        {
            GetReady();
            var actRes = controller.Put(1, new SupplierModel() { Id = 1, Name = "Networks doo", Address = "Aleja lipa 33", TownId = 1 });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod] //PASSED
        public void ChangeSupplierTown()
        {
            GetReady();
            var actRes = controller.Put(1, new SupplierModel() { Id = 1, Name="BH Pošta",Address="Obala Kulina Bana 8",TownId=2});
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteById()
        {
            GetReady();
            var actRes = controller.Delete(2);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void DeleteByWrongId()
        {
            GetReady();
            var actRes = controller.Delete(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }


    }
}
