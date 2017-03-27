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
    public class TestInvoicesController
    {
        InvoicesController controller = new InvoicesController();
        HttpConfiguration config = new HttpConfiguration();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/invoice");

        void GetReady(string currentRoute = "api/{controller}/{id}")
        {
            var route = config.Routes.MapHttpRoute("default", currentRoute);
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "invoice" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllInvoices()
        {
            TestHelper.InitDatabase(); GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetSInvoiceById()
        {
            GetReady();
            var actRes = controller.Get(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInvoiceByWrongId()
        {
            GetReady();
            var actRes = controller.Get(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetInvoicesByCustomer()
        { 
            GetReady("api/{ controller}/customer/{id}");
            var actRes = controller.GetByCustomer(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInvoicesByCustomerBad()
        {
            GetReady("api/{ controller}/customer/{id}");
            var actRes = controller.GetByCustomer(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetInvoicesByAgent()
        {
            GetReady("api/{ controller}/Agent/{id}");
            var actRes = controller.GetByAgent(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInvoicesByAgentBad()
        {
            GetReady("api/{ controller}/Agent/{id}");
            var actRes = controller.GetByAgent(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void PostInvoiceByAgentGood()
        {
            GetReady("api/{controller}/Agent/{id}");
            var actRes = controller.Post(new InvoiceModel()
            {
                InvoiceNo = "125GH",
                Date = new DateTime(2011, 6, 10),
                ShippedOn = new DateTime(2011, 7, 10),
                Status = 0,
                //SubTotal = 100,
                Vat = 17,
                //VatAmount = 18,
                //Total = 117,
                ShipperId = 1,
                AgentId = 1,
                CustomerId = 1,
                Shipping = 19
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostInvoiceByAgentBad()
        {
            GetReady("api/{controller}/Agent/{id}");
            var actRes = controller.Post(new InvoiceModel()
            {
                InvoiceNo = "125GH",
                Date = new DateTime(2011, 6, 10),
                ShippedOn = new DateTime(2011, 7, 10),
                Status = 0,
                //SubTotal = 100,
                Vat = 17,
                //VatAmount = 18,
                //Total = 117,
                ShipperId = 1,
                AgentId = 99,
                CustomerId = 1,
                Shipping = 19
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }


        [TestMethod]
        public void PostInvoiceByCustomerGood()
        {
            GetReady("api/{controller}/Customer/{id}");
            var actRes = controller.Post(new InvoiceModel()
            {
                InvoiceNo = "125GH",
                Date = new DateTime(2011, 6, 10),
                ShippedOn = new DateTime(2011, 7, 10),
                Status = 0,
                //SubTotal = 100,
                Vat = 17,
                //VatAmount = 18,
                //Total = 117,
                ShipperId = 1,
                AgentId = 1,
                CustomerId = 1,
                Shipping = 19
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostInvoiceByCustomerBad()
        {
            GetReady("api/{controller}/Customer/{id}");

            var actRes = controller.Post(new InvoiceModel()
            {
                InvoiceNo = "125GH",
                Date = new DateTime(2011, 6, 10),
                ShippedOn = new DateTime(2011, 7, 10),
                Status = 0,
                //SubTotal = 100,
                Vat = 17,
                //VatAmount = 18,
                //Total = 117,
                ShipperId = 1,
                AgentId = 1,
                CustomerId = 99,
                Shipping = 19
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeInvoicesData()
        {
            GetReady();
            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "12N5GH",
                Date = new DateTime(2011, 4, 10),
                ShippedOn = new DateTime(2011, 8, 10),
                Status = 0,
                //SubTotal = 1000,
                Vat = 17,
                //VatAmount = 128,
                //Total = 1175,
                Shipping = 100,
                ShipperId = 1,
                AgentId = 1,
                CustomerId = 1
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeInvoicesAgentGood()
        {
            GetReady();
            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "12N5GH",
                Date = new DateTime(2011, 4, 10),
                ShippedOn = new DateTime(2011, 8, 10),
                Status = 0,
                //SubTotal = 1000,
                Vat = 17,
                //VatAmount = 128,
                //Total = 1175,
                Shipping = 100,
                ShipperId = 1,
                AgentId = 2,
                CustomerId = 1
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeInvoicesAgentBad()
        {
            GetReady();
            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "12N5GH",
                Date = new DateTime(2011, 4, 10),
                ShippedOn = new DateTime(2011, 8, 10),
                Status = 0,
                //SubTotal = 1000,
                Vat = 17,
                //VatAmount = 128,
                //Total = 1175,
                Shipping = 100,
                ShipperId = 1,
                AgentId = 99,
                CustomerId = 1
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeInvoicesCustomerGood()
        {
            GetReady();
            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "12N5GH",
                Date = new DateTime(2011, 4, 10),
                ShippedOn = new DateTime(2011, 8, 10),
                Status = 0,
                //SubTotal = 1000,
                Vat = 17,
                //VatAmount = 128,
                //Total = 1175,
                Shipping = 100,
                ShipperId = 1,
                AgentId = 1,
                CustomerId = 2
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeInvoicesCustomerBad()
        {
            GetReady();
            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "12N5GH",
                Date = new DateTime(2011, 4, 10),
                ShippedOn = new DateTime(2011, 8, 10),
                Status = 0,
                //SubTotal = 1000,
                Vat = 17,
                //VatAmount = 128,
                //Total = 1175,
                Shipping = 100,
                ShipperId = 1,
                AgentId = 1,
                CustomerId = 99
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

      

        [TestMethod]
        public void DeleteByWrongId()
        {
            GetReady();
            var actRes = controller.Delete(99);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteById()
        {
            GetReady();
            var actRes = controller.Delete(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
