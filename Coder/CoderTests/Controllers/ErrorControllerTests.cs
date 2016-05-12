using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Coder.Controllers.Tests
{

    [TestClass()]
    public class ErrorControllerTests
    {
        private ErrorController controller = new ErrorController();

        [TestMethod()]
        public void ForbiddenTest()
        {
            //Arrange

            //Act
            ViewResult result = controller.Forbidden() as ViewResult;

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void BadRequestTest()
        {
            //Arrange

            //Act
            ViewResult result = controller.BadRequest() as ViewResult;

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void NotFoundTest()
        {
            //Arrange

            //Act
            ViewResult result = controller.NotFound() as ViewResult;

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void UnauthorizedTest()
        {
            //Arrange

            //Act
            ViewResult result = controller.Unauthorized() as ViewResult;

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void InternalServerErrorTest()
        {
            //Arrange

            //Act
            ViewResult result = controller.InternalServerError() as ViewResult;

            //Assert

            Assert.IsNotNull(result);
        }
    }
}