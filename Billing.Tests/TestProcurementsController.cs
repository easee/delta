using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.Api.Controllers;
using System.Web.Http;
using System.Threading;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using Billing.Api.Models;
using System;
using System.Collections.Generic;

namespace Billing.Tests
{
    [TestClass]
    public class TestProcurementsController
    {
        ProcurementsController controller = new ProcurementsController();
        HttpConfiguration config = new HttpConfiguration();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/procurements");

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "procurements" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllProcurements()
        {
            TestHelper.InitDatabase(); GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetProcurementById()
        {
            GetReady();
            var actRes = controller.Get(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetProcurementByWrongId()
        {
            GetReady();
            var actRes = controller.Get(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetProcurementByProductName()
        {
            GetReady();
            var actRes = controller.Get("Laptop Dell 2866");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        //----------------------------------------
        [TestMethod]
        //public void GetProcurementByProductNameBad()
        //{
        //    GetReady();
        //    var actRes = controller.Get("teeest");
        //    var response = actRes.ExecuteAsync(CancellationToken.None).Result;

        //    Assert.IsNull(response.Content);
        //}
        public void GetProcurementsByWrongProduct()
        {
            GetReady();
            var response = controller.GetByProduct(99).ExecuteAsync(CancellationToken.None).Result;
            List<ProcurementModel> dataSet;
            var content = response.TryGetContentValue<List<ProcurementModel>>(out dataSet);
            Assert.AreEqual(0, dataSet.Count);
        }
        //----------------------------------------

        [TestMethod]
        public void GetProcurementByDocument()
        {
            GetReady();
            var actRes = controller.GetByDoc("2055 - 2");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        //----------------------------------------------------
        [TestMethod]
        public void GetProcurementsByWrongDoc()
        {
            GetReady();
            var response = controller.GetByDoc("999999").ExecuteAsync(CancellationToken.None).Result;
            List<ProcurementModel> dataSet;
            var content = response.TryGetContentValue<List<ProcurementModel>>(out dataSet);
            Assert.AreEqual(0, dataSet.Count);
        }

        //public void GetProcurementByDocumentBad()
        //{
        //    GetReady();
        //    var actRes = controller.GetByDoc("teeest");
        //    var response = actRes.ExecuteAsync(CancellationToken.None).Result;

        //    Assert.IsNull(response.Content);
        //}

        [TestMethod]
        public void PostProcurementProductGood()
        {
            GetReady();
            var actRes = controller.Post(new ProcurementModel() { Price = 458.4, Total = 2000, Quantity = 2, ProductId = 1, SupplierId = 1, Date = DateTime.UtcNow});
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostProcurementProductBad()
        {
            GetReady();
            var actRes = controller.Post(new ProcurementModel() { Price = 458.4, Total = 2000, Quantity = 2, ProductId = 99, SupplierId = 99 });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeProcurementDataGood()
        {
            GetReady();
            var actRes = controller.Put(1, new ProcurementModel() { Id = 1, Document = "272/01", Date = new DateTime(2016, 12, 22), ProductId = 2, SupplierId = 2, Quantity = 2, Price = 399 });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeProcurementProductGood()
        {
            GetReady();
            var actRes = controller.Put(1, new ProcurementModel() { Id = 1, Price = 458.4, Total = 2000, Quantity = 2, SupplierId = 1, ProductId = 2, Date = DateTime.UtcNow });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeProcurementProductBad()
        {
            GetReady();
            var actRes = controller.Put(99, new ProcurementModel() { Id = 1, Price = 458.4, Total = 2000, Quantity = 2, SupplierId = 1, ProductId = 1 });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteProcurementById()
        {
            GetReady();
            var actRes = controller.Delete(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteProcurementByWrongId()
        {
            GetReady();
            var actRes = controller.Delete(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }




        [TestMethod]
        public void DeleteSingle()
        {
            GetReady();
            var actRes = controller.Delete(2);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        //[TestMethod]
        //public void DeleteWidow()
        //{
        //    GetReady();
        //    var actRes = controller.Delete(1);
        //    var response = actRes.ExecuteAsync(CancellationToken.None).Result;

        //    Assert.IsFalse(response.IsSuccessStatusCode);
        //}

    }
}