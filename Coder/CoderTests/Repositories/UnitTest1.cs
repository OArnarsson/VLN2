using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoderTests.Repositories
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //The main purpose of this test was to see if these tests actiually work....

            //Arrange:
            const int testId1 = 3;
            const int testId2 = 3;
            //ApplicationDbContext db = new ApplicationDbContext();
            //Act:

            //Assert: 
            Assert.AreEqual(testId1, testId2);
        }
    }
}
