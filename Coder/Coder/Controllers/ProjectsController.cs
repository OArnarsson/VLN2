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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using MvcSiteMapProvider.Web.Mvc.Filters;
using Coder.Repositories;

namespace Coder.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ProjectsRepository projectsRepository;
        private readonly CoursesRepository coursesRepository;

        public ProjectsController()
        {
            projectsRepository = new ProjectsRepository(db);
            coursesRepository = new CoursesRepository(db);
        }

        // GET: Projects
        public ActionResult Index()
        {
            var projects = projectsRepository.GetProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator"));

            if (projects == null)
            {
                return View();
            }
            else
            {
                return View(projects.ToList());
            }
        }

        // GET: Projects/Details/5
        [SiteMapTitle("title")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = projectsRepository.GetProjectById(id);

            if (project == null)
            {
                return HttpNotFound();
            }
            
            if (!coursesRepository.IsInCourse(project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            if (!coursesRepository.IsTeacherInAnyCourse(User.Identity.GetUserId()) && !User.IsInRole("Administrator"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (User.IsInRole("Administrator"))
            {
                ViewBag.CourseId = new SelectList(coursesRepository.GetAllCourses(), "Id", "Name");
            }
            else
            {
                ViewBag.CourseId = new SelectList(coursesRepository.GetCoursesForTeacherWithTeacherRole(User.Identity.GetUserId()), "Id", "Name");
            }

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                projectsRepository.AddProject(project);
                return RedirectToAction("Index");
            }

            if (User.IsInRole("Administrator"))
            {
                ViewBag.CourseId = new SelectList(coursesRepository.GetAllCourses(), "Id", "Name", project.CourseId);
            }
            else
            {
                ViewBag.CourseId = new SelectList(coursesRepository.GetCoursesForTeacherWithTeacherRole(User.Identity.GetUserId()), "Id", "Name", project.CourseId);
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [SiteMapTitle("title")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!coursesRepository.IsTeacherInCourse(projectsRepository.GetProjectById(id).CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Administrator"))
            {
                ViewBag.CourseId = new SelectList(coursesRepository.GetAllCourses(), "Id", "Name", project.CourseId);
            }
            else
            {
                ViewBag.CourseId = new SelectList(coursesRepository.GetCoursesForTeacherWithTeacherRole(User.Identity.GetUserId()), "Id", "Name", project.CourseId);
            }

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                if (!coursesRepository.IsTeacherInCourse(project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                projectsRepository.UpdateState(EntityState.Modified, project);
                projectsRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            if (User.IsInRole("Administrator"))
            {
                ViewBag.CourseId = new SelectList(coursesRepository.GetAllCourses(), "Id", "Name", project.CourseId);
            }
            else
            {
                ViewBag.CourseId = new SelectList(coursesRepository.GetCoursesForTeacherWithTeacherRole(User.Identity.GetUserId()), "Id", "Name", project.CourseId);
            }

            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!coursesRepository.IsTeacherInCourse(projectsRepository.GetProjectById(id).CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Project project = projectsRepository.GetProjectById(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!coursesRepository.IsTeacherInCourse(projectsRepository.GetProjectById(id).CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            projectsRepository.RemoveProject(projectsRepository.GetProjectById(id));
            
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
