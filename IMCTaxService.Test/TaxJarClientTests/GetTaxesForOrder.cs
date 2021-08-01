using AutoMapper;
using IMCTaxService.Entities.TaxClientEntities;
using IMCTaxService.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Test.TaxJarClientTests
{
    [TestClass]
    public class GetTaxesForOrder
    {
        private TaxJarTaxClient GetClientForBadArgumentTests()
        {
            var mockMapper = new Mock<IMapper>();
            var mockRestClient = new Mock<IRestClient>();
            var mockJsonConvert = new Mock<IJsonConvert>();
            return new TaxJarTaxClient(mockMapper.Object, mockRestClient.Object, mockJsonConvert.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullOrder_ExceptionThrown()
        {
            var client = GetClientForBadArgumentTests();
            client.GetTaxesForOrder(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullToAddress_ExceptionThrown()
        {
            var order = new TaxClientOrder()
            {
                FromAddress = null,
                ToAddress = new TaxClientAddress()
            };
            var client = GetClientForBadArgumentTests();
            client.GetTaxesForOrder(order);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullFromAddress_ExceptionThrown()
        {
            var order = new TaxClientOrder()
            {
                FromAddress = new TaxClientAddress(),
                ToAddress = null
            };
            var client = GetClientForBadArgumentTests();
            client.GetTaxesForOrder(order);
        }

        [TestMethod]
        public void GivenTaxJarAPI_Returns200_ValidResponse()
        {
            //TODO follow same patterns i've demonstrated understanding of elsewhere. Mocking dependencies, injecting them,
            //mocking responses, asserting correct behavior and output.
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException))]
        public void GivenTaxJarAPI_ReturnsUnexpectedStatus_ExceptionThrown()
        {
            var response = new RestResponse()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = "shouldn't matter what this content is if code is (correctly) not deserializing response."
            };
            var mockMapper = new Mock<IMapper>();
            var mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(s => s.Execute(It.IsAny<IRestRequest>()))
                .Returns(response);

            var mockJsonConvert = new Mock<IJsonConvert>();

            var client = new TaxJarTaxClient(mockMapper.Object, mockRestClient.Object, mockJsonConvert.Object);

            var taxClientOrder = new TaxClientOrder()
            {
                FromAddress = new TaxClientAddress(),
                ToAddress = new TaxClientAddress()
            };
            client.GetTaxesForOrder(taxClientOrder);
        }
    }
}
