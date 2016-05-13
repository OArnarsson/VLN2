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
using MvcSiteMapProvider.Web.Mvc.Filters;
using System.IO;
using System.Diagnostics;
using Coder.Repositories;
using Coder.Helpers;
using Microsoft.AspNet.Identity;
using Coder.Models.ViewModels;

namespace Coder.Controllers
{
    [ValidateInput(false)]
    public class ProjectTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ProjectTasksRepository projectTasksRepository;
        private readonly CoursesRepository coursesRepository;
        private readonly ProjectsRepository projectsRepository;
        private readonly SubmissionsRepository submissionsRepository;
        private readonly UserCoursesRepository userCoursesRepostiory;

        public ProjectTasksController()
        {
            projectTasksRepository = new ProjectTasksRepository(db);
            coursesRepository = new CoursesRepository(db);
            projectsRepository = new ProjectsRepository(db);
            submissionsRepository = new SubmissionsRepository(db);
            userCoursesRepostiory = new UserCoursesRepository(db);
        }

        // GET: ProjectTasks
        public ActionResult Index()
        {
            if (coursesRepository.IsTeacherInAnyCourse(User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                ViewBag.IsTeacher = true;
            }
            if (User.IsInRole("Administrator"))
            {
                return View(projectTasksRepository.GetAllProjectTasks());
            }

            return View(projectTasksRepository.GetAllProjectTasksForUserId(User.Identity.GetUserId()));
        }

        // GET: ProjectTasks/Details/5
        [SiteMapTitle("title")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request!");
            }

            ProjectTask projectTask = projectTasksRepository.GetProjectTaskById(id);

            if (projectTask == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Not found!");
            }
            
            if (!coursesRepository.IsInCourse(projectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }
            

            bool isTeacher = (coursesRepository.IsTeacherInCourse(projectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")));

            ViewBag.IsTeacher = isTeacher;
            ViewBag.AllUsers = (from u in userCoursesRepostiory.GetUserCoursesByCourseId(projectTask.Project.CourseId)
                                where u.CoderRole == CoderRole.Student
                                select u.ApplicationUser).ToList();
            ViewBag.BestSubmission = submissionsRepository.GetBestUserSubmissionForTask(id.Value, User.Identity.GetUserId());

            if (isTeacher || User.IsInRole("Administrator"))
            {
                ViewBag.Submissions = submissionsRepository.GetSubmissionsForProjectTaskId(id.Value);
            }
            else
            {
                if (DateTime.Now < projectTask.Project.Start && !coursesRepository.IsAssistantTeacherInCourse(projectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")))
                {
                    throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
                }
                ViewBag.Submissions = submissionsRepository.GetSubmissionsForUserId(User.Identity.GetUserId());
            }
            
            return View(projectTask);
        }

        // GET: ProjectTasks/Create
        public ActionResult Create()
        {
           
            if (!coursesRepository.IsTeacherInAnyCourse(User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }

            // Gets all projects if admin, else just the user's projects

            if (!User.IsInRole("Administrator"))
            {
                var allProjects = (projectsRepository.GetProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator"))).ToList();

                var teacherProjects = (from p in allProjects
                                       where p.Course.UserCourses.FirstOrDefault(x => x.UserId == User.Identity.GetUserId()).CoderRole == Coder.Models.Entity.CoderRole.Teacher
                                       select p).ToList();

                ViewBag.ProjectId = new SelectList(teacherProjects, "Id", "Name");
            }
            else ViewBag.ProjectId = new SelectList(projectsRepository.GetProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")), "Id", "Name");


            return View();
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectTask projectTask, FormCollection form)
        {
            if (!coursesRepository.IsTeacherInAnyCourse(User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }

            if (ModelState.IsValid)
            {
                foreach (var file in form.GetValue("files").AttemptedValue.Split(','))
                {
                    if (String.IsNullOrEmpty(file.Trim()))
                    {
                        continue;
                    }
                    projectTasksRepository.AddFilesRequired(new FileRequired { Name = file.Trim(), ProjectTaskId = projectTask.Id });
                }

                for (int i = 0; i < form.Count; i++)
                {
                    var key = form.Keys[i];

                    if (key.StartsWith("test") && key.EndsWith("output"))
                    {
                        var counter = key.Split('_')[1];
                        string input = form.GetValue("test_" + counter + "_input").AttemptedValue.Replace("&quot;", "\"");
                        string output = form.GetValue("test_" + counter + "_output").AttemptedValue.Replace("&quot;", "\"");
                        projectTasksRepository.AddTaskTests(new TaskTest { Input = input, Output = output, ProjectTaskId = projectTask.Id });
                    }
                }

                projectTasksRepository.AddProjectTask(projectTask);
                projectTasksRepository.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.ProjectId = new SelectList(projectsRepository.GetProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")), "Id", "Name", projectTask.ProjectId);
            return View(projectTask);
        }

        // GET: ProjectTasks/Edit/5
        [SiteMapTitle("title")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request!");
            }

            ProjectTask projectTask = projectTasksRepository.GetProjectTaskById(id);

            if (projectTask == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Not found!");
            }

            if (!coursesRepository.IsTeacherInCourse(projectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }

            if (!User.IsInRole("Administrator"))
            {
                var allProjects = (projectsRepository.GetProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator"))).ToList();

                var teacherProjects = (from p in allProjects
                                       where p.Course.UserCourses.FirstOrDefault(x => x.UserId == User.Identity.GetUserId()).CoderRole == Coder.Models.Entity.CoderRole.Teacher
                                       select p).ToList();

                ViewBag.ProjectId = new SelectList(teacherProjects, "Id", "Name");
            }
            else ViewBag.ProjectId = new SelectList(projectsRepository.GetProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")), "Id", "Name");

            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectTask projectTask, FormCollection form)
        {
            if (!coursesRepository.IsTeacherInCourse(projectTask.Id, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }

            if (ModelState.IsValid)
            {
                projectTasksRepository.UpdateState(EntityState.Modified, projectTask);
                projectTasksRepository.SaveChanges();

                projectTasksRepository.RemoveAllTaskTestsForProjectTask(projectTask);
                projectTasksRepository.RemoveAllFilesRequiredForProjectTask(projectTask);

                foreach (var file in form.GetValue("files").AttemptedValue.Split(','))
                {
                    if (String.IsNullOrEmpty(file.Trim()))
                    {
                        continue;
                    }
                    projectTasksRepository.AddFilesRequired(new FileRequired { Name = file.Trim(), ProjectTaskId = projectTask.Id });
                }

                for (int i = 0; i < form.Count; i++)
                {
                    var key = form.Keys[i];

                    if (key.StartsWith("test") && key.EndsWith("output"))
                    {
                        var counter = key.Split('_')[1];
                        string input = form.GetValue("test_" + counter + "_input").AttemptedValue.Replace("&quot;", "\"");
                        string output = form.GetValue("test_" + counter + "_output").AttemptedValue.Replace("&quot;", "\"");
                        projectTasksRepository.AddTaskTests(new TaskTest { Input = input, Output = output, ProjectTaskId = projectTask.Id });
                    }
                }

                projectTasksRepository.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectTask.ProjectId);
            return View(projectTask);
        }

        /*public ActionResult EditTests(int? id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTests()
        {
            return View();
        }*/

        // GET: ProjectTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request!");
            }

            ProjectTask projectTask = projectTasksRepository.GetProjectTaskById(id);

            if (projectTask == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Not found!");
            }

            if (!coursesRepository.IsTeacherInCourse(projectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }

            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!coursesRepository.IsTeacherInCourse(id, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }

            projectTasksRepository.RemoveProjectTask(projectTasksRepository.GetProjectTaskById(id));
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

        public ActionResult SaveUploadedFile(int Id)
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                ProjectTask task = projectTasksRepository.GetProjectTaskById(Id);
                // Checking if all files are required
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;

                    if (!task.FilesRequired.Any(i => i.Name == fName)) {
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.StatusCode = 400;
                        Response.StatusDescription = "File not allowed";
                        Response.ContentType = "application/json";
                        return Json(new { Message = "File is not in this Task. See required files above." });
                    }
                }

                if (Request.Files.Count == task.FilesRequired.Count)
                {
                    Submission newSubmission = new Submission { ProjectTaskId = task.Id, Created = DateTime.Now, ApplicationUsers = new List<ApplicationUser>()};

                    // Add group members to submission
                    foreach (var k in Request.Form.Keys)
                    {
                        string key = k.ToString();
                        var userId = Request.Form[key];
                        if (key.StartsWith("user") && !string.IsNullOrEmpty(userId))
                        {
                            newSubmission.ApplicationUsers.Add(db.Users.FirstOrDefault(i => i.Id == userId));
                            db.Submissions.Add(newSubmission);
                        }
                    }
                    // Add current user to submission
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = db.Users.FirstOrDefault(i => i.Id == currentUserId);
                    newSubmission.ApplicationUsers.Add(currentUser);
                    db.Submissions.Add(newSubmission);
                    db.SaveChanges();

                    // All files are valid and all files are there, so we save them
                    foreach (string fileName in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[fileName];

                        if (file != null && file.ContentLength > 0)
                        {
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Uploads\\Submissions", Server.MapPath(@"\")));

                            string pathString = System.IO.Path.Combine(originalDirectory.ToString(), newSubmission.Id.ToString());

                            var fileName1 = Path.GetFileName(file.FileName);

                            bool isExists = System.IO.Directory.Exists(pathString);

                            if (!isExists)
                                System.IO.Directory.CreateDirectory(pathString);

                            var path = string.Format("{0}\\{1}", pathString, file.FileName);
                            file.SaveAs(path);
                        }
                    }
                    SubmissionsHelper subHelper = new SubmissionsHelper(db);
                    subHelper.createCppSubmission(task, newSubmission);
                }
                else
                {
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.StatusCode = 400;
                    Response.StatusDescription = "Test requires " + task.FilesRequired.Count + " but the submission only had " + Request.Files.Count;
                    Response.ContentType = "application/json";
                    return Json(new { Message = "Test requires " + task.FilesRequired.Count + " files, but the submission only had " + Request.Files.Count + ".", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
        

        // GET: ProjectTasks/Grade/5
        public ActionResult Grade(int? id)
        {
            if (id == null)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request!");
            }

            ProjectTask projectTask = projectTasksRepository.GetProjectTaskById(id);

            if (!User.IsInRole("Administrator") && !(coursesRepository.IsTeacherInCourse(projectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) || coursesRepository.IsAssistantTeacherInCourse(projectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator"))))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }
            var courseId = projectTask.Project.Course.Id;
            var users = (from uc in db.UserCourses
                         where uc.CourseId == courseId
                         select uc.ApplicationUser).ToList();

            List<GradeTaskViewModel> rows = new List<GradeTaskViewModel>();

            var userId = User.Identity.GetUserId();
            foreach (var u in users)
            {
                var best = submissionsRepository.GetBestUserSubmissionForTask(id.Value, u.Id);
                rows.Add(new GradeTaskViewModel()
                {
                    ApplicationUser = u,
                    Submission = best,
                    GradeProjectTask = db.GradeProjectTasks.FirstOrDefault(i => i.ProjectTaskId == id.Value && i.UserId == u.Id)
                });
            }
            ViewBag.ProjectTask = projectTask;
            return View(rows);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grade(FormCollection form)
        {
            var projectTaskId = 0;
            int.TryParse(form["projectTaskId"], out projectTaskId);
            var projectTask = projectTasksRepository.GetProjectTaskById(projectTaskId);

            // TODO: Check if course exists
            if (!User.IsInRole("Administrator") && !(coursesRepository.IsTeacherInCourse(projectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) || coursesRepository.IsAssistantTeacherInCourse(projectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator"))))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }

            projectTasksRepository.RemoveAllGradesForProjectTask(projectTask);

            foreach (var k in form.Keys)
            {
                string key = k.ToString();
                var grade = 0;
                int.TryParse(form[key], out grade);

                if (key.StartsWith("grade_"))
                {
                    var userId = key.Split('_')[1];
                    projectTask.GradeProjectTasks.Add(new GradeProjectTask
                    {
                        UserId = userId,
                        Grade = grade,
                        ProjectTaskId = projectTaskId
                    });
                }
            }

            projectTasksRepository.UpdateState(EntityState.Modified, projectTask);
            projectTasksRepository.SaveChanges();
            return RedirectToAction("Details", "ProjectTasks", new { Id = projectTaskId });
        }
    }
}
