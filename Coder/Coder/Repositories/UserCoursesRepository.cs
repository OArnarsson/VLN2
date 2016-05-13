﻿using Coder.Models;
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

        public UserCoursesRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        public List<UserCourse> GetUserCoursesByCourseId(int? courseId)
        {
            return db.UserCourses.Where(i => i.CourseId == courseId).ToList();
        }

        public List<UserCourse> GetUserCoursesByUserId(string userId)
        {
            return db.UserCourses.Where(i => i.UserId == userId).ToList();
        }

        public void RemoveAllUserCoursesForCourseId(int courseId)
        {
            foreach (var x in db.UserCourses.Where(i => i.CourseId == courseId))
            {
                db.UserCourses.Remove(x);
            }
        }

        public void RemoveAllUserCoursesForUserId(string userId)
        {
            foreach (var x in db.UserCourses.Where(i => i.UserId == userId))
            {
                db.UserCourses.Remove(x);
            }
        }

        public void AddUserCourse(UserCourse userCourse)
        {
            db.UserCourses.Add(userCourse);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}