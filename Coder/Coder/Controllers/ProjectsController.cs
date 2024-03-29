﻿using System;
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
using Coder.Models.ViewModels;

namespace Coder.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ProjectsRepository projectsRepository;
        private readonly CoursesRepository coursesRepository;
        private readonly SubmissionsRepository submissionsRepository;

        public ProjectsController()
        {
            projectsRepository = new ProjectsRepository(db);
            coursesRepository = new CoursesRepository(db);
            submissionsRepository = new SubmissionsRepository(db);
        }

        // GET: Projects
        public ActionResult Index()
        {
            var projects = projectsRepository.GetProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator"));

            if (coursesRepository.IsTeacherInAnyCourse(User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                ViewBag.IsTeacher = true;
            }

            if (projects == null)
            {
                return View();
            }
            return View(projects.ToList());
        }

        // GET: Projects/Details/5
        [SiteMapTitle("title")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new HttpException((int) HttpStatusCode.BadRequest, "Bad request!");
            }

            var project = projectsRepository.GetProjectById(id);

            if (project == null)
            {
                throw new HttpException((int) HttpStatusCode.NotFound, "Not found!");
            }

            if (!coursesRepository.IsInCourse(project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                throw new HttpException((int) HttpStatusCode.Forbidden, "Forbidden!");
            }

            if (coursesRepository.IsTeacherInCourse(project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                ViewBag.IsTeacher = true;
            }
            else
            {
                // Checking if the project hasn't started yet
                if (DateTime.Now < project.Start && !User.IsInRole("Administrator") && !coursesRepository.IsAssistantTeacherInCourse(project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")))
                {
                    throw new HttpException((int) HttpStatusCode.Forbidden, "Forbidden!");
                }
            }

            double totalGrade = 0;
            double totalGrades = 0;
            double totalValue = 0;
            foreach (var task in project.ProjectTasks)
            {
                var gradeProjectTask = task.GradeProjectTasks.Where(g => g.UserId == User.Identity.GetUserId()).FirstOrDefault();
                if (gradeProjectTask != null)
                {
                    totalGrade += task.GradeProjectTasks.Where(g => g.UserId == User.Identity.GetUserId()).FirstOrDefault().Grade*task.Value;
                    totalValue += task.Value;
                    totalGrades++;
                }
            }
            if (totalGrades == project.ProjectTasks.Count && (project.ProjectTasks.Count > 0))
            {
                ViewBag.Grade = Math.Round(totalGrade/totalValue, 2);
            }
            else
            {
                ViewBag.Grade = "All tasks haven't been graded yet.";
            }

            var taskViewModels = new List<TaskViewModel>();
            foreach (var task in project.ProjectTasks)
            {
                taskViewModels.Add(new TaskViewModel
                {
                    BestSubmission = submissionsRepository.GetBestUserSubmissionForTask(task.Id, User.Identity.GetUserId()),
                    Task = task
                });
            }

            var view = new ProjectDetailsViewModel
            {
                Project = project,
                Tasks = taskViewModels
            };

            return View(view);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            if (!coursesRepository.IsTeacherInAnyCourse(User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                throw new HttpException((int) HttpStatusCode.Forbidden, "Forbidden!");
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
                throw new HttpException((int) HttpStatusCode.BadRequest, "Bad request!");
            }

            var project = projectsRepository.GetProjectById(id);

            if (project != null)
            {
                if (!coursesRepository.IsTeacherInCourse(project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
                {
                    throw new HttpException((int) HttpStatusCode.Forbidden, "Forbidden!");
                }
            }
            else
            {
                throw new HttpException((int) HttpStatusCode.NotFound, "Not found!");
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
                    throw new HttpException((int) HttpStatusCode.Forbidden, "Forbidden!");
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
                throw new HttpException((int) HttpStatusCode.BadRequest, "Bad request!");
            }

            var project = projectsRepository.GetProjectById(id);

            if (project != null)
            {
                if (!coursesRepository.IsTeacherInCourse(project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
                {
                    throw new HttpException((int) HttpStatusCode.Forbidden, "Forbidden!");
                }
            }
            else
            {
                throw new HttpException((int) HttpStatusCode.NotFound, "Not found!");
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
                throw new HttpException((int) HttpStatusCode.Forbidden, "Forbidden!");
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