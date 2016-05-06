using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Coder.Models;
using Coder.Models.Entity;
using Coder.Models.ViewModels;
using MvcSiteMapProvider.Web.Mvc.Filters;
using Microsoft.AspNet.Identity;
using Coder.Repositories;

namespace Coder.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly CoursesRepository coursesRepository;
        private readonly UserCoursesRepository userCoursesRepository;
        private readonly UsersRepository usersRepository;

        public CoursesController()
        {
            coursesRepository = new CoursesRepository(db);
            userCoursesRepository = new UserCoursesRepository(db);
            usersRepository = new UsersRepository(db);
        }

        // GET: Courses
        public ActionResult Index()
        {
            var courses = coursesRepository.GetCoursesForUser(User.Identity.GetUserId());

            if (User.IsInRole("Administrator"))
            {
                courses = coursesRepository.GetAllCourses();
            }
            
            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        [SiteMapTitle("title")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = coursesRepository.GetCourseFromId(id);

            if (course == null)
            {
                return HttpNotFound();
            }
            
            if (!coursesRepository.IsInCourse(id, User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            course = coursesRepository.GetCourseFromId(id, User.Identity.GetUserId(), User.IsInRole("Administrator"));

            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Title,Start,End")] Course course)
        {
            if (ModelState.IsValid)
            {
                coursesRepository.AddCourse(course);
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        [SiteMapTitle("title")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = coursesRepository.GetCourseFromId(id, User.Identity.GetUserId(), User.IsInRole("Administrator"));

            if (course == null)
            {
                return HttpNotFound();
            }

            if (!coursesRepository.IsInCourse(id, User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            CourseViewModel courseViewModel = new CourseViewModel(course);
            courseViewModel.UserCourses = userCoursesRepository.GetUserCoursesByCourseId(id);
            courseViewModel.ApplicationUsers = usersRepository.GetAllUsers();
           
            return View(courseViewModel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseViewModel courseViewModel, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                Course course = coursesRepository.GetCourseFromId(courseViewModel.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator"));
                course.Name = courseViewModel.Name;
                course.Description = courseViewModel.Description;
                course.Title = courseViewModel.Title;
                course.Start = courseViewModel.Start;
                course.End = courseViewModel.End;

                userCoursesRepository.RemoveAllUserCoursesForCourse(course);

                foreach (var x in getUserCoursesFromFormCollection(form, course.Id))
                {
                    userCoursesRepository.AddUserCourse(x);
                }

                coursesRepository.UpdateState(EntityState.Modified, course);

                // Saved it here instead of after each UserCourse add in the foreach loop above.
                // Normally I have db.SaveChanges() just in the repository functions
                userCoursesRepository.SaveChanges();

                return RedirectToAction("Details", new { course.Id });
            }

            courseViewModel.UserCourses = db.UserCourses.Where(i => i.CourseId == courseViewModel.CourseId).ToList();
            courseViewModel.ApplicationUsers = db.Users.ToList();
            return View(courseViewModel);
        }

        public List<UserCourse> getUserCoursesFromFormCollection(FormCollection form, int courseId)
        {
            var userCourses = new List<UserCourse>();
            for (int i = 0; i < form.Count; i++)
            {
                var key = form.Keys[i];

                if (key.StartsWith("User_") && !string.IsNullOrEmpty(form.GetValue(key).AttemptedValue))
                {
                    var val = int.Parse(form.GetValue(key).AttemptedValue.ToString());
                    var userId = key.Split('_')[1];
                    userCourses.Add(new UserCourse { UserId = userId.ToString(), CourseId = courseId, CoderRole = (CoderRole)val });
                }
            }

            return userCourses;
        }

        // GET: Courses/Delete/5
        //[SiteMapTitle("title")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!User.IsInRole("Administrator"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            
            Course course = coursesRepository.GetCourseFromId(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            coursesRepository.RemoveCourse(coursesRepository.GetCourseFromId(id));
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
