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
using System.Net;

namespace IMCTaxService.Test.TaxControllerTests
{
    [TestClass]
    public class RateTests
    {
        private TaxController GetTaxControllerForBadRequest()
        {
            var mockLogger = new Mock<ILogger<TaxController>>();
            var mockMapper = new Mock<IMapper>();
            var mockFactory = new Mock<IFactory<ITaxClient, UserType>>();
            return new TaxController(mockFactory.Object, mockLogger.Object, mockMapper.Object);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("    ")]
        [DataRow("A")]
        [DataRow("ABC")]
        //test is a bit fragile if underlying impl of badrequest changes. don't know a better way to do this.
        public void GivenBadCountry_BadRequestReturned(string country)
        {
            var controller = GetTaxControllerForBadRequest();
            var result = controller.TaxRates(country, null, null, null, "45069") as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode.Value);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("    ")]
        [DataRow(null)]
        public void GivenBadZip_BadRequestReturned(string zip)
        {
            var controller = GetTaxControllerForBadRequest();
            var result = controller.TaxRates("US", null, null, null, zip) as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode.Value);
        }

        [TestMethod]
        [DataRow("US", "city", "OH", "Street Drive", "45069")]
        public void NoResultsReturned_404Response(string country, string city, string state, string street, string zipcode)
        {
            var mockLogger = new Mock<ILogger<TaxController>>();
            var mockMapper = new Mock<IMapper>();

            var mockFactory = new Mock<IFactory<ITaxClient, UserType>>();
            var mockClient = new Mock<ITaxClient>();
            mockFactory.Setup(s => s.GetInstance(It.IsAny<UserType>()))
                .Returns(mockClient.Object);

            var controller = new TaxController(mockFactory.Object, mockLogger.Object, mockMapper.Object);
            var result = controller.TaxRates(country, city, state, street, zipcode) as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode.Value);
        }

        [TestMethod]
        [DataRow("US", "city", "OH", "Street Drive", "45069")]
        public void ResultsFound_200Response(string country, string city, string state, string street, string zipcode)
        {
            var mockLogger = new Mock<ILogger<TaxController>>();
            var mockMapper = new Mock<IMapper>();
            var response = new TaxClientRate();

            var mockFactory = new Mock<IFactory<ITaxClient, UserType>>();
            var mockClient = new Mock<ITaxClient>();
            mockClient
                .Setup(s => s.GetRates(It.IsAny<Location>()))
                .Returns(response);
            mockFactory
                .Setup(s => s.GetInstance(It.IsAny<UserType>()))
                .Returns(mockClient.Object);

            var controller = new TaxController(mockFactory.Object, mockLogger.Object, mockMapper.Object);
            var result = controller.TaxRates(country, city, state, street, zipcode) as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode.Value);
        }

        [TestMethod]
        [DataRow("US", "city", "OH", "Street Drive", "45069")]
        public void ResultsFound_MapperResponseReturned(string country, string city, string state, string street, string zipcode)
        {
            var taxClientResponse = new TaxClientRate();
            var expectedServiceResponse = new TaxRateResponse();

            var mockLogger = new Mock<ILogger<TaxController>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.
                Setup(s => s.Map<TaxRateResponse>(It.Is<object>(t => t == taxClientResponse)))
                .Returns(expectedServiceResponse);

            var mockFactory = new Mock<IFactory<ITaxClient, UserType>>();
            var mockClient = new Mock<ITaxClient>();
            mockClient
                .Setup(s => s.GetRates(It.IsAny<Location>()))
                .Returns(taxClientResponse);
            mockFactory
                .Setup(s => s.GetInstance(It.IsAny<UserType>()))
                .Returns(mockClient.Object);

            var controller = new TaxController(mockFactory.Object, mockLogger.Object, mockMapper.Object);
            var result = controller.TaxRates(country, city, state, street, zipcode) as ObjectResult;
            var actualServiceResponse = result.Value;
            Assert.AreEqual(expectedServiceResponse, actualServiceResponse);
        }

    }
}
