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
    public class UserCoursesRepository
    {
        private readonly ApplicationDbContext db;

        /*
        * Initialization.
        */
        public UserCoursesRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        /*
        * Fetches users from course with specific ID.
        */
        public List<UserCourse> GetUserCoursesByCourseId(int? courseId)
        {
            return db.UserCourses.Where(i => i.CourseId == courseId).ToList();
        }

        /*
        * Fetches courses from user with specific ID.
        */
        public List<UserCourse> GetUserCoursesByUserId(string userId)
        {
            return db.UserCourses.Where(i => i.UserId == userId).ToList();
        }

        /*
        * Removes all users from a course with specific ID.
        */
        public void RemoveAllUserCoursesForCourseId(int courseId)
        {
            foreach (var x in db.UserCourses.Where(i => i.CourseId == courseId))
            {
                db.UserCourses.Remove(x);
            }
        }

        /*
        * Removes all courses from a user with specific ID.
        */
        public void RemoveAllUserCoursesForUserId(string userId)
        {
            foreach (var x in db.UserCourses.Where(i => i.UserId == userId))
            {
                db.UserCourses.Remove(x);
            }
        }

        /*
        * Adds a connection between user and course to the database.
        */
        public void AddUserCourse(UserCourse userCourse)
        {
            db.UserCourses.Add(userCourse);
        }

        /*
        * Saves changes to the database.
        */
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}