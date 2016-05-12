using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Coder.Controllers;

namespace Coder.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        private HomeController controller = new HomeController();

        [TestMethod()]
        public void IndexTest()
        {
            // Arrange
           
            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}