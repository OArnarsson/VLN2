﻿using System;
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
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcSiteMapProvider.Web.Mvc.Filters;

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
        [SiteMapTitle("title")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userViewModel = new UserViewModel(user);
            userViewModel.Courses = db.Courses.ToList();
            userViewModel.UserCourses = db.UserCourses.Where(i => i.UserId == user.Id).ToList();

            return View(userViewModel);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            CreateUserViewModel userViewModel = new CreateUserViewModel()
            {
                Courses = db.Courses.ToList()
            };

            return View(userViewModel);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel userViewModel, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);

                var newUser = new ApplicationUser
                {
                    Name = userViewModel.Name,
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email
                };

                var result = await UserManager.CreateAsync(newUser, userViewModel.Password);
            
                var userCourses = getUserCoursesFromFormCollection(form, newUser.Id);

                foreach (var i in userCourses)
                {
                    db.UserCourses.Add(i);
                }

                db.Users.Add(newUser);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }

        // GET: Users/Edit/5
        [SiteMapTitle("title")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userViewModel = new UserViewModel(user);
            userViewModel.Courses = db.Courses.ToList();
            userViewModel.UserCourses = db.UserCourses.Where(i => i.UserId == user.Id).ToList();
            
            return View(userViewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel userViewModel, FormCollection form)
        {
            if (userViewModel.Password != userViewModel.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "The password must be at least 6 characters long.");
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.FirstOrDefault(i => i.Id == userViewModel.UserId);
                user.Name = userViewModel.Name;
                user.Email = userViewModel.Email;

                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);

                if (!String.IsNullOrEmpty(userViewModel.Password))
                {
                    UserManager.RemovePassword(user.Id);
                    UserManager.AddPassword(user.Id, userViewModel.Password);
                }

                foreach (var x in db.UserCourses.Where(i => i.UserId == user.Id))
                {
                    db.UserCourses.Remove(x);
                }

                foreach (var x in getUserCoursesFromFormCollection(form, user.Id))
                {
                    db.UserCourses.Add(x);
                }

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }

        public List<UserCourse> getUserCoursesFromFormCollection(FormCollection form, string userId)
        {
            var userCourses = new List<UserCourse>();
            for (int i = 0; i < form.Count; i++)
            {
                var key = form.Keys[i];

                if (key.StartsWith("Course_") && !string.IsNullOrEmpty(form.GetValue(key).AttemptedValue))
                {
                    var val = int.Parse(form.GetValue(key).AttemptedValue.ToString());
                    var courseId = int.Parse(key.Split('_')[1]);
                    userCourses.Add(new UserCourse { UserId = userId, CourseId = courseId, CoderRole = (CoderRole)val });
                }
            }
            return userCourses;
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
            ApplicationUser user = db.Users.Find(id);
            foreach (var c in db.UserCourses.Where(i => i.UserId == id))
            {
                db.UserCourses.Remove(c);
            }
            db.Users.Remove(user);
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
