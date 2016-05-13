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

        /*
        * Initialization.
        */
        public CoursesRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        /*
        * Fetches all the courses from the database.
        */
        public IEnumerable<Course> GetAllCourses()
        {
            return db.Courses.ToList();
        }

        /*
        * Fetches the courses the user is enrolled in.
        */
        public IEnumerable<Course> GetCoursesForUser(string userId)
        {
            return (from c in db.Courses
                    join u in db.UserCourses on c.Id equals u.CourseId
                    where u.UserId == userId
                    select c).ToList();
        }

        /*
        * Fetches the courses where the user is marked as a student.
        */
        public IEnumerable<Course> GetCoursesForStudentWithStudentRole(string userId)
        {
            return GetCoursesForUserWithRole(userId, CoderRole.Student);
        }

        /*
        * Fetches the courses where the user is marked as an assistant teacher.
        */
        public IEnumerable<Course> GetCoursesForTeacherWithAssistantTeacherRole(string userId)
        {
            return GetCoursesForUserWithRole(userId, CoderRole.TeachingAssistant);
        }

        /*
        * Fetches the courses where the user is marked as a teacher.
        */
        public IEnumerable<Course> GetCoursesForTeacherWithTeacherRole(string userId)
        {
            return GetCoursesForUserWithRole(userId, CoderRole.Teacher);
        }

        /*
        * Fetches the courses the user is enrolled in, and returns the user's role in the course as well.
        */
        public IEnumerable<Course> GetCoursesForUserWithRole(string userId, CoderRole role)
        {
            return (from c in db.Courses
                    join u in db.UserCourses on c.Id equals u.CourseId
                    where u.UserId == userId && u.CoderRole == role
                    select c).ToList();
        }

        /*
        * Fetches a course with specified ID, if the user is enrolled or has administrative rights.
        */
        public Course GetCourseFromId(int? id, string userId, bool isAdmin)
        {
            if (IsInCourse(id, userId, isAdmin))
            {
                return GetCourseFromId(id);
            }

            return null;
        }

        /*
        * Used when admin can only call, like GET delete
        */
        public Course GetCourseFromId(int? id)
        {
            return db.Courses.FirstOrDefault(i => i.Id == id);
        }

        /*
        * Adds a new course to the database.
        */
        public void AddCourse(Course course)
        {
            db.Courses.Add(course);
            db.SaveChanges();
        }

        /*
        * Removes a course from the database.
        */
        public void RemoveCourse(Course course)
        {
            db.Courses.Remove(course);
            db.SaveChanges();
        }

        /*
        * Updates the course in the database.
        */
        public void UpdateState(EntityState state, Course course)
        {
            db.Entry(course).State = state;
        }

        /*
        * Checks if the user is enrolled in the course, or has administrative rights.
        */
        public bool IsInCourse(int? id, string userId, bool isAdmin)
        {
            if (isAdmin)
            {
                return true;
            }
            return db.UserCourses.Any(x => x.CourseId == id && x.UserId == userId);
        }

        /*
        * Checks if the user is enrolled as a teacher in the course, or has administrative rights.
        */
        public bool IsTeacherInCourse(int? id, string userId, bool isAdmin)
        {
            return HasRoleInCourse(id, userId, isAdmin, CoderRole.Teacher);
        }


        /*
        * Checks if the user is enrolled in the course and has a role, or has administrative rights.
        */
        public bool IsAssistantTeacherInCourse(int? id, string userId, bool isAdmin)
        {
           return HasRoleInCourse(id, userId, isAdmin, CoderRole.TeachingAssistant);
        }

        /*
        * Checks if the user is enrolled in the course, or has administrative rights.
        */
        public bool HasRoleInCourse(int? id, string userId, bool isAdmin, CoderRole role)
        {
            if (!isAdmin)
            {
                UserCourse userCourse = db.UserCourses.Where(i => i.CourseId == id && i.UserId == userId).FirstOrDefault();

                if (userCourse != null)
                {
                    return (userCourse.CoderRole == role);
                }
            }
            return false;
        }

        /*
        * Checks if the user is enrolled as a teacher in any course, or has administrative rights.
        */
        public bool IsTeacherInAnyCourse(string userId, bool isAdmin)
        {
            return HasSuperRoleInAnyCourse(userId, isAdmin, CoderRole.Teacher);
        }

        /*
        * Checks if the user is enrolled as an assistant teacher in any course, or has administrative rights.
        */
        public bool IsAssistantTeacherInAnyCourse(string userId, bool isAdmin)
        {
            return HasSuperRoleInAnyCourse(userId, isAdmin, CoderRole.TeachingAssistant);
        }

        /*
        * Checks if the user is enrolled in any course, or has administrative rights.
        */
        public bool HasSuperRoleInAnyCourse(string userId, bool isAdmin, CoderRole role)
        {
            return isAdmin || db.UserCourses.Where(i => i.UserId == userId).Any(x => x.CoderRole == role);
        }
    }
}