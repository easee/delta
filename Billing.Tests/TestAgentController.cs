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
    public class TestAgentController
    {
        AgentsController controller = new AgentsController();
        HttpConfiguration config = new HttpConfiguration();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/agents");

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "agents" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllAgents()
        {
            TestHelper.InitDatabase(); GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetAgentById()
        {
            GetReady();
            var actRes = controller.Get(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetAgentByWrongId()
        {
            GetReady();
            var actRes = controller.Get(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void PostAgentGood()
        {
            GetReady();
            var actRes = controller.Post(new AgentModel() { Name = "Anur" });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        //[TestMethod]
        //public void ChangeAgentName()
        //{
        //    GetReady();
        //    var actRes = controller.Put(1, new AgentModel() { Id = 1, Name = "New name for old agent" });
        //    var response = actRes.ExecuteAsync(CancellationToken.None).Result;

        //    Assert.IsTrue(response.IsSuccessStatusCode);
        //}

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

            Assert.IsNull(response.Content);
        }
    }
}
