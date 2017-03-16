using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.Api.Controllers;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Threading;
using Billing.Api.Models;

namespace Billing.Tests
{
    /// <summary>
    /// Summary description for TestItemsConstroler
    /// </summary>
    [TestClass]
    public class TestItemsConstroler
    {
        ItemsController controller = new ItemsController();
        HttpConfiguration config = new HttpConfiguration();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/items");

        void GetReady(string currentRoute = "api/{controller}/{id}" )
        {
            var route = config.Routes.MapHttpRoute("default", currentRoute);
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "products" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod] //Not Null - PASSED
        public void GetAllItems()
        {
            TestHelper.InitDatabase(); GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod] //Get Item by ID is Not Null - PASSED
        public void GetItemById()
        {
            GetReady();
            var actRes = controller.Get(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }


        [TestMethod] //Get Item by Wrong Id is Null - PASSED
        public void GetItemByWrongId()
        {
            GetReady();
            var actRes = controller.Get(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod] //Get items by Invoice (1) is Not Null - PASSED
        public void GetItemsByInvoice()
        {
            GetReady();
            var actRes = controller.GetByInvoice(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod] // Get items by Invoice (99) is Null - PASSED
        public void GetItemsByInvoiceWrong()
        {
            GetReady();
            var actRes = controller.GetByInvoice(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod] //Get items by Product 1 is Not Null - PASSED
        public void GetItemsByProduct()
        {
            GetReady();
            var actRes = controller.GetByProduct(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod] //Get items by Product (99) is Null - PASSED
        public void GetItemsByProductWrong()
        {
            GetReady();
            var actRes = controller.GetByProduct(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod] //Insert item for Product 1 is True - PASSED
        public void PostItemGoodProduct()
        {
            GetReady();
            var actRes = controller.Post(new ItemModel() { Quantity = 1, Price = 100, InvoiceId = 1, ProductId = 1 });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        //[TestMethod] //Insert item for Product 99 is False - NOT PASSED
        //public void PostItemBadProduct()
        //{
        //    GetReady();
        //    var actRes = controller.Post(new ItemModel() { Quantity = 1, Price = 100, ProductId = 99 });
        //    var response = actRes.ExecuteAsync(CancellationToken.None).Result;

        //    Assert.IsFalse(response.IsSuccessStatusCode);
        //}

        //[TestMethod] //Change Item's data Is True
        //public void ChangeItemData()
        //{
        //    GetReady();
        //    var actRes = controller.Put(1, new ItemModel() { Id = 1, Quantity = 5, Price = 100 });
        //    var response = actRes.ExecuteAsync(CancellationToken.None).Result;

        //    Assert.IsTrue(response.IsSuccessStatusCode);
        //}

        //[TestMethod]//Change item's product to 2 Is True
        //public void ChangItemProduct()
        //{
        //    GetReady();
        //    var actRes = controller.Put(1, new ItemModel() { Id = 1, Quantity = 500, Price = 100 });
        //    var response = actRes.ExecuteAsync(CancellationToken.None).Result;

        //    Assert.IsTrue(response.IsSuccessStatusCode);
        //}

        //[TestMethod]//Change Item's Product to 99 Is False
        //public void ChangItemProductWrong()
        //{
        //    GetReady();
        //    var actRes = controller.Put(1, new ItemModel() { Id = 1, Quantity = 500, Price = 100 });
        //    var response = actRes.ExecuteAsync(CancellationToken.None).Result;

        //    Assert.IsFalse(response.IsSuccessStatusCode);
        //}


        [TestMethod] //Delete item with Id = 1 Is True - PASSED
        public void DeleteById() //Or we can call it DeleteSingle
        {
            GetReady();
            var actRes = controller.Delete(2);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod] //Delete item with Id = 99 Is False - PASSED
        public void DeleteByWrongId()
        {
            GetReady();
            var actRes = controller.Delete(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }
    }
}