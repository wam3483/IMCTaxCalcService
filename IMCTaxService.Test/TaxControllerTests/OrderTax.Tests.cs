using AutoMapper;
using IMCTaxService.Controllers;
using IMCTaxService.Entities.AppObjects;
using IMCTaxService.Entities.ServiceDTO;
using IMCTaxService.Entities.TaxClientEntities;
using IMCTaxService.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace IMCTaxService.Test.TaxControllerTests
{
    [TestClass]
    public class OrderTaxTests
    {
        private TaxController GetTaxControllerForBadRequest()
        {
            var mockLogger = new Mock<ILogger<TaxController>>();
            var mockMapper = new Mock<IMapper>();
            var mockFactory = new Mock<IFactory<ITaxClient, UserType>>();
            return new TaxController(mockFactory.Object, mockLogger.Object, mockMapper.Object);
        }
        [TestMethod]
        public void GivenNullRequest_BadRequestReturned()
        {
            var controller = GetTaxControllerForBadRequest();
            var response = controller.OrderTax(null) as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode.Value);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [DataRow("A")]
        [DataRow("ABC")]
        public void GivenBadFromCountry_BadRequestReturned(string fromCountry)
        {
            var controller = GetTaxControllerForBadRequest();
            var orderRequest = new OrderRequest()
            {
                FromAddress = new Address()
                {
                    Country = fromCountry
                }
            };
            var response = controller.OrderTax(orderRequest) as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode.Value);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [DataRow("A")]
        [DataRow("ABC")]
        public void GivenBadToCountry_BadRequestReturned(string toCountry)
        {
            var controller = GetTaxControllerForBadRequest();
            var orderRequest = new OrderRequest()
            {
                FromAddress = new Address()
                {
                    Country = "US"
                },
                ToAddress = new Address()
                {
                    Country = toCountry
                }
            };
            var response = controller.OrderTax(orderRequest) as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode.Value);
        }

        [TestMethod]
        public void GivenTaxClientResponse_ExpectedDataReturned()
        {
            var svcRequest = new OrderRequest()
            {
                FromAddress = new Address() { Country = "US" },
                ToAddress = new Address() { Country = "US" }
            };
            var taxClientResponse = new TaxClientOrderTax();
            var expectedSvcResponse = new OrderResponse();


            var mockLogger = new Mock<ILogger<TaxController>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(s => s.Map<OrderResponse>(It.Is<object>(t => t == taxClientResponse)))
                .Returns(expectedSvcResponse);

            var mockFactory = new Mock<IFactory<ITaxClient, UserType>>();
            var mockClient = new Mock<ITaxClient>();
            mockClient
                .Setup(s => s.GetTaxesForOrder(It.IsAny<TaxClientOrder>()))
                .Returns(taxClientResponse);
            mockFactory
                .Setup(s => s.GetInstance(It.IsAny<UserType>()))
                .Returns(mockClient.Object);

            var controller = new TaxController(mockFactory.Object, mockLogger.Object, mockMapper.Object);
            var actualSvcResponse = controller.OrderTax(svcRequest) as ObjectResult;

            //know some people are opposed to multiple asserts. Feel it's acceptable here as i'm validating
            //the http code and the payload as part of a correct response payload.
            Assert.AreEqual(expectedSvcResponse, actualSvcResponse.Value);
            Assert.AreEqual((int)HttpStatusCode.OK, actualSvcResponse.StatusCode.Value);
        }
    }
}
