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
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcSiteMapProvider.Web.Mvc.Filters;

namespace Coder.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserStore<ApplicationUser> store;
        private UserManager<ApplicationUser> userManager;

        public UsersController()
        {
            store = new UserStore<ApplicationUser>(db);
            userManager = new UserManager<ApplicationUser>(store);
        }

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
                var newUser = new ApplicationUser
                {
                    Name = userViewModel.Name,
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email
                };

                var result = await userManager.CreateAsync(newUser, userViewModel.Password);
            
                var userCourses = getUserCoursesFromFormCollection(form, newUser.Id);

                foreach (var i in userCourses)
                {
                    db.UserCourses.Add(i);
                }

                if (userViewModel.Admin)
                {
                    userManager.AddToRole(newUser.Id, "Administrator");
                }

                db.Users.Add(newUser);
                db.SaveChanges();

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
            userViewModel.Admin = userManager.IsInRole(user.Id, "Administrator");
            
            return View(userViewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel userViewModel, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.FirstOrDefault(i => i.Id == userViewModel.UserId);
                user.Name = userViewModel.Name;
                user.Email = userViewModel.Email;

                if (!String.IsNullOrEmpty(userViewModel.Password))
                {
                    userManager.RemovePassword(user.Id);
                    userManager.AddPassword(user.Id, userViewModel.Password);
                }

                foreach (var x in db.UserCourses.Where(i => i.UserId == user.Id))
                {
                    db.UserCourses.Remove(x);
                }

                foreach (var x in getUserCoursesFromFormCollection(form, user.Id))
                {
                    db.UserCourses.Add(x);
                }

                if (userViewModel.Admin)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                }
                else
                {
                    userManager.RemoveFromRole(user.Id, "Administrator");
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
