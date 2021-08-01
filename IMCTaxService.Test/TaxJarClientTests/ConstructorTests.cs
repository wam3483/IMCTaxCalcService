using AutoMapper;
using IMCTaxService.Proxy;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Test.TaxJarClientTests
{
    [TestClass]
    public class ConstructorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorGivenNullMapper_ThrowsException()
        {
            var mockRestClient = new Mock<IRestClient>();
            var mockConvert = new Mock<IJsonConvert>();
            new TaxJarTaxClient(null, mockRestClient.Object, mockConvert.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorGivenNullRestClient_ThrowsException()
        {
            var mockMapper = new Mock<IMapper>();
            var mockConvert = new Mock<IJsonConvert>();
            new TaxJarTaxClient(mockMapper.Object, null, mockConvert.Object);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorGivenNullJsonConverter_ThrowsException()
        {
            var mockMapper = new Mock<IMapper>();
            var mockRestClient = new Mock<IRestClient>();
            new TaxJarTaxClient(mockMapper.Object, mockRestClient.Object, null);
        }
    }
}
