using Coder.Models;
using Coder.Models.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Coder.Repositories
{
    public class CoursesRepository
    {
        private readonly ApplicationDbContext db;
        public CoursesRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        public IEnumerable<Course> GetCoursesForUser(string userId)
        {
            return (from c in db.Courses
                    join u in db.UserCourses on c.Id equals u.CourseId
                    where u.UserId == userId
                    select c).ToList();
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return db.Courses.ToList();
        }

        public Course GetCourseFromId(int? id)
        {
            return db.Courses.Find(id);
        }
    }
}

