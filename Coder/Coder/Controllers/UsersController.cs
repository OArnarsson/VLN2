using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Coder.Models;
using Coder.Models.ViewModels;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Coder.Models.Entity;

namespace Coder.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // TODO: check if ID is valid
            // db.Users.Single(u => u.Id == id)
            ApplicationUser applicationUser = db.Users.Find(id);

            UserViewModel userViewModel = new UserViewModel()
            {
                Courses = db.Courses.ToList(),
                CurrentUser = applicationUser
            };

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(userViewModel);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            UserViewModel userViewModel = new UserViewModel()
            {
                Courses = db.Courses.ToList(),
                CurrentUser = new ApplicationUser()
            };

            return View(userViewModel);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel userViewModel, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                userViewModel.CurrentUser.UserCourses = new List<UserCourse>();
                for (int i = 0; i < form.Count; i++)
                {
                    var key = form.Keys[i];

                    if (key.StartsWith("Course_") && !string.IsNullOrEmpty(form.GetValue(key).AttemptedValue.ToString()))
                    {
                        var val = int.Parse(form.GetValue(key).AttemptedValue.ToString());
                        var courseId = int.Parse(key.Split('_')[1]);
                        userViewModel.CurrentUser.UserCourses.Add(new UserCourse { CourseId = courseId, CoderRole = (CoderRole)val });
                    }
                }

                userViewModel.CurrentUser.UserName = userViewModel.CurrentUser.Email;
                db.Users.Add(userViewModel.CurrentUser);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                
                return RedirectToAction("Index");
            }

            return View(userViewModel);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser applicationUser = db.Users.Find(id);

            UserViewModel userViewModel = new UserViewModel()
            {
                Courses = db.Courses.ToList(),
                CurrentUser = applicationUser
            };

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(userViewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel userViewModel, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                userViewModel.CurrentUser.UserCourses = new List<UserCourse>();

                userViewModel.CurrentUser.UserCourses.RemoveAll(x => x.UserId == userViewModel.CurrentUser.Id);

                for (int i = 0; i < form.Count; i++)
                {
                    var key = form.Keys[i];

                    if (key.StartsWith("Course_") && !string.IsNullOrEmpty(form.GetValue(key).AttemptedValue.ToString()))
                    {
                        var val = int.Parse(form.GetValue(key).AttemptedValue.ToString());
                        var courseId = int.Parse(key.Split('_')[1]);
                        userViewModel.CurrentUser.UserCourses.Add(new UserCourse { UserId = userViewModel.CurrentUser.Id, CourseId = courseId, CoderRole = (CoderRole)val });
                    }
                }

                userViewModel.CurrentUser.UserName = userViewModel.CurrentUser.Email;
                db.Entry(userViewModel.CurrentUser).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                
                return RedirectToAction("Index");
            }
            return View(userViewModel.CurrentUser);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
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
