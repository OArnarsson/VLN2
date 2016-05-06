using Coder.Models;
using Coder.Models.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        
        public IEnumerable<Course> GetAllCourses()
        {
            return db.Courses.ToList();
        }

        public IEnumerable<Course> GetCoursesForUser(string userId)
        {
            return (from c in db.Courses
                    join u in db.UserCourses on c.Id equals u.CourseId
                    where u.UserId == userId
                    select c).ToList();
        }

        public Course GetCourseFromId(int? id, string userId, bool isAdmin)
        {
            if (IsInCourse(id, userId, isAdmin))
            {
                return GetCourseFromId(id);
            }

            return null;
        }

        // Used when admin can only call, like GET delete
        public Course GetCourseFromId(int? id)
        {
            return db.Courses.FirstOrDefault(i => i.Id == id);
        }

        public void AddCourse(Course course)
        {
            db.Courses.Add(course);
            db.SaveChanges();
        }

        public void RemoveCourse(Course course)
        {
            db.Courses.Remove(course);
            db.SaveChanges();
        }

        public void UpdateState(EntityState state, Course course)
        {
            db.Entry(course).State = state;
        }
        
        // Helper function, perhabs should be in a Service layer
        public bool IsInCourse(int? id, string userId, bool isAdmin)
        {
            if (isAdmin)
            {
                return true;
            }
            return db.UserCourses.Any(x => x.CourseId == id && x.UserId == userId);
        }

        public bool IsTeacherInCourse(int? id, string userId)
        {
            return (db.UserCourses.Where(i => i.CourseId == id && i.UserId == userId).FirstOrDefault().CoderRole == CoderRole.Teacher);
        }
    }
}

