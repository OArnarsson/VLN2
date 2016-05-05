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

namespace Coder.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.projects = (from p in db.Projects
                                where p.CourseId == id.Value
                                select p).ToList();
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
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            CourseViewModel courseViewModel = new CourseViewModel(course);
            courseViewModel.UserCourses = db.UserCourses.Where(i => i.CourseId == course.Id).ToList();
            courseViewModel.ApplicationUsers = db.Users.ToList();
           
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
                Course course = db.Courses.FirstOrDefault(i => i.Id == courseViewModel.CourseId);
                course.Name = courseViewModel.Name;
                course.Description = courseViewModel.Description;
                course.Title = courseViewModel.Title;
                course.Start = courseViewModel.Start;
                course.End = courseViewModel.End;

                foreach (var x in db.UserCourses.Where(i => i.CourseId == course.Id))
                {
                    db.UserCourses.Remove(x);
                }

                foreach (var x in getUserCoursesFromFormCollection(form, course.Id))
                {
                    db.UserCourses.Add(x);
                }

                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
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
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
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
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
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
