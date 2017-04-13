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
    public class TestShippersController
    {
        ShippersController controller = new ShippersController();
        HttpConfiguration config = new HttpConfiguration();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/shipper");

        void GetReady(string currentRoute = "api/{controller}/{id}")
        {
            var route = config.Routes.MapHttpRoute("default", currentRoute);
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "shipper" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllShippers()
        {
            TestHelper.InitDatabase(); GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetShipperById()
        {
            GetReady();
            var actRes = controller.Get(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetShipperByWrongId()
        {
            GetReady();
            var actRes = controller.Get(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetShipperByName()
        {
            GetReady();
            var actRes = controller.Get("DHL");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void PostShipperGood()
        {
            GetReady();
            var actRes = controller.Post(new ShipperModel() { Name = "Bruce Lee", Address = "Ulica 25", TownId = 1 });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeShipperName()
        {
            GetReady();
            var actRes = controller.Put(1, new ShipperModel() { Id = 1, Name = "New name for old shipper", Address="Aleja od lipe 33", TownId=1 });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeShipperTown()
        {
            GetReady();
            var actRes = controller.Put(1, new ShipperModel() { Id = 1, Name = "New name for old shipper", Address = "Aleja od lipe 33", TownId = 2 });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteByWrongId()
        {
            GetReady();
            var actRes = controller.Delete(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteById()
        {
            GetReady();
            var actRes = controller.Delete(2);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
