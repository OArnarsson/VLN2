using Coder.Models;
using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Repository
{
    public class CoursesRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Course> getAllCoursesByUserId(string userId)
        {
            return (from c in db.Courses
                    where c.UserCourses.Any(i => i.UserId == userId)
                    select c);
        }
    }
}

