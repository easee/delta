using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.Api.Controllers;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Security.Principal;
using System.Threading;
using Billing.Repository;

namespace Billing.Tests.ReportTests
{
    [TestClass]
    public class TestProductsbyCategory
    {
        ProductsByCategoryController controller = new ProductsByCategoryController();
        HttpConfiguration config = new HttpConfiguration();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/productsbycategory");

        [TestInitialize]
        public void Initializing()
        {
            TestHelper.InitDatabase();
        }

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "productsbycategory" } });
            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }
        //[TestMethod]
        //public void ProductsByCategoryByWrongId()
        //{
        //    GetReady();
        //    var actRes = controller.Get(9999);
        //    var response = actRes.ExecuteAsync(CancellationToken.None).Result;
        //    Assert.IsNull(response.Content);
        //}
        [TestMethod]
        public void ProductsByCategoryById()
        {
            GetReady();
            var actRes = controller.Get(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

    }
}
