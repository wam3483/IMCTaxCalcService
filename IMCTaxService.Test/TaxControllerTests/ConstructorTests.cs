using AutoMapper;
using IMCTaxService.Controllers;
using IMCTaxService.Entities.AppObjects;
using IMCTaxService.Proxy;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Test.TaxControllerTests
{
    [TestClass]
    public class ConstructorTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenCtorNullMapper_ArgumentThrown()
        {
            var mockFactory = new Mock<IFactory<ITaxClient, UserType>>();
            var mockLogger = new Mock<ILogger<TaxController>>();
            new TaxController(mockFactory.Object, mockLogger.Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenCtorNullLogger_ArgumentThrown()
        {
            var mockFactory = new Mock<IFactory<ITaxClient, UserType>>();
            var mockMapper = new Mock<IMapper>();
            new TaxController(mockFactory.Object, null, mockMapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenCtorNullFactory_ArgumentThrown()
        {
            var mockLogger = new Mock<ILogger<TaxController>>();
            var mockMapper = new Mock<IMapper>();
            new TaxController(null, mockLogger.Object, mockMapper.Object);
        }
    }
}
