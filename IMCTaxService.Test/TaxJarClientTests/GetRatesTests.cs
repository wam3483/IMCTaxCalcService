using AutoMapper;
using IMCTaxService.Entities.TaxClientEntities;
using IMCTaxService.Entities.TaxJarDTO;
using IMCTaxService.Proxy;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IMCTaxService.Test.TaxJarClientTests
{
    [TestClass]
    public class GetRatesTests
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
        public void GivenNullLocation_ThrowsException()
        {
            var client = GetClientForBadArgumentTests();
            client.GetRates(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("  ")]
        public void GivenBadZipcode_ThrowsException(string zipcode)
        {
            var client = GetClientForBadArgumentTests();
            Location location = new Location()
            {
                Zip = zipcode,
                Country = "US"
            };
            client.GetRates(location);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("  ")]
        [DataRow("A")]
        [DataRow("ABC")]
        public void GivenBadCountry_ThrowsException(string country)
        {
            var client = GetClientForBadArgumentTests();
            Location location = new Location()
            {
                Zip = "45042",
                Country = country
            };
            client.GetRates(location);
        }

        [TestMethod]
        //
        [DataRow(null, "city", "street")]
        [DataRow("", "city", "street")]
        [DataRow("  ", "city", "street")]
        //
        [DataRow("OH", null, "street")]
        [DataRow("OH", "", "street")]
        [DataRow("OH", "  ", "street")]
        //
        [DataRow("OH", "city", null)]
        [DataRow("OH", "city", "")]
        [DataRow("OH", "city", "  ")]
        public void AssertQueryParamsPassedCorrectly(string state, string city, string street)
        {
            var mockMapper = new Mock<IMapper>();
            bool statePassed = !string.IsNullOrWhiteSpace(state);
            bool cityPassed = !string.IsNullOrWhiteSpace(city);
            bool streetPassed = !string.IsNullOrWhiteSpace(street);
            var mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(s => s.Execute(It.IsAny<IRestRequest>()))
                .Returns<IRestRequest>((restRequest) =>
                {
                    Assert.IsTrue(statePassed || !restRequest.Parameters.Any(s => s.Name == "state"));
                    Assert.IsTrue(cityPassed || !restRequest.Parameters.Any(s => s.Name == "city"));
                    Assert.IsTrue(streetPassed || !restRequest.Parameters.Any(s => s.Name == "street"));

                    return new RestResponse()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                });

            var mockJsonConvert = new Mock<IJsonConvert>();
            mockJsonConvert.Setup(s => s.Deserialize<TaxJarRateResponse>(It.IsAny<string>()))
                .Returns(new TaxJarRateResponse());

            var client = new TaxJarTaxClient(mockMapper.Object, mockRestClient.Object, mockJsonConvert.Object);

            var location = new Location()
            {
                Country = "US",
                Zip = "45042",
                City = city,
                State = state,
                Street = street
            };
            client.GetRates(location);
            //ensure asserts in callback actually executed to ensure params present when passed, and absent when not passed.
            mockRestClient.Verify(s => s.Execute(It.IsAny<IRestRequest>()), Times.Once);
        }

        [TestMethod]
        public void GivenValidZip_PassedInResourceCorrectly()
        {
            var location = new Location()
            {
                Country = "US",
                Zip = "45042",
                City = "city",
                State = "state",
                Street = "street"
            };

            var mockMapper = new Mock<IMapper>();
            var mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(s => s.Execute(It.IsAny<IRestRequest>()))
                .Returns<IRestRequest>((restRequest) =>
                {
                    var actualZip = restRequest.Resource.Remove(0, "/rates/".Length);
                    Assert.AreEqual(location.Zip, actualZip);

                    return new RestResponse()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                });

            var mockJsonConvert = new Mock<IJsonConvert>();
            mockJsonConvert.Setup(s => s.Deserialize<TaxJarRateResponse>(It.IsAny<string>()))
                .Returns(new TaxJarRateResponse());

            var client = new TaxJarTaxClient(mockMapper.Object, mockRestClient.Object, mockJsonConvert.Object);

            client.GetRates(location);
            //ensure asserts in callback actually executed to ensure params present when passed, and absent when not passed.
            mockRestClient.Verify(s => s.Execute(It.IsAny<IRestRequest>()), Times.Once);
        }

        [TestMethod]
        public void GivenTaxJarAPIReturns200_ReturnsCorrectly()
        {
            //TODO i'd follow same patterns demonstrated below, assert response from api with 200 results in response from my client.
        }

        [TestMethod]
        public void GivenTaxJarAPIReturns404_NullReturned()
        {
            var location = new Location()
            {
                Country = "US",
                Zip = "45042",
                City = "city",
                State = "state",
                Street = "street"
            };
            var response = new RestResponse()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Content = "shouldn't matter what this content is if code is (correctly) not deserializing response."
            };
            var mockMapper = new Mock<IMapper>();
            var mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(s => s.Execute(It.IsAny<IRestRequest>()))
                .Returns(response);

            var mockJsonConvert = new Mock<IJsonConvert>();
            mockJsonConvert.Setup(s => s.Deserialize<TaxJarRateResponse>(It.IsAny<string>()))
                .Returns(new TaxJarRateResponse());

            var client = new TaxJarTaxClient(mockMapper.Object, mockRestClient.Object, mockJsonConvert.Object);

            var clientResult = client.GetRates(location);
            Assert.IsNull(clientResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException))]
        public void GivenTaxJarAPIReturnsUnexpectedResponseCode_ExceptionThrow()
        {
            var location = new Location()
            {
                Country = "US",
                Zip = "45042",
                City = "city",
                State = "state",
                Street = "street"
            };
            var response = new RestResponse()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            var mockMapper = new Mock<IMapper>();
            var mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(s => s.Execute(It.IsAny<IRestRequest>()))
                .Returns(response);

            var mockJsonConvert = new Mock<IJsonConvert>();
            mockJsonConvert.Setup(s => s.Deserialize<TaxJarRateResponse>(It.IsAny<string>()))
                .Returns(new TaxJarRateResponse());

            var client = new TaxJarTaxClient(mockMapper.Object, mockRestClient.Object, mockJsonConvert.Object);

            client.GetRates(location);
        }
    }
}
