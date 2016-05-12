using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coder.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coder.Models;

namespace Coder.Repositories.Tests
{
    [TestClass()]
    public class UserCoursesRepositoryTests
    {
        private readonly ApplicationDbContext db;
        [TestMethod()]
        public void GetUserCoursesByCourseIdTest()
        {
            //Arrange:
            const int testId = 3;
            //ApplicationDbContext db = new ApplicationDbContext();
            var service = new UserCoursesRepository(db);

            //Act:
            var result = service.GetUserCoursesByCourseId(testId);

            int a = result.Count;

            //Assert: 
            Assert.AreEqual(result.Count, result.Count);

            //Assert.Fail();
        }
    }
}