﻿using Coder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coder.Models.Entity;
using Coder.Repositories;
using Microsoft.AspNet.Identity;
using Coder.Helpers;

namespace Coder.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private CommentsRepository commentsRepo;
        private ProjectTasksRepository projectTasksRepo;
        private UsersRepository usersRepo;
        private CoursesRepository coursesRepo;

        public CommentsController()
        {
            commentsRepo = new CommentsRepository(db);
            projectTasksRepo = new ProjectTasksRepository(db);
            usersRepo = new UsersRepository(db);
            coursesRepo = new CoursesRepository(db);
        }

        // GET: Comments
        public ActionResult CommentsForProjectTask(int projectTaskId)
        {
            return View("Comments", commentsRepo.getCommentsForProjectTaskId(projectTaskId));
        }

        // POST: Comments/Create
        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            comment.UserId = User.Identity.GetUserId();
            comment.Created = DateTime.Now;
            comment.ProjectTask = projectTasksRepo.GetProjectTaskById(comment.ProjectTaskId);
            comment.ApplicationUser = usersRepo.GetUserById(User.Identity.GetUserId());
            commentsRepo.AddComment(comment);

            if (Request.IsAjaxRequest())
            {
                bool canDeleteComment = (User.Identity.GetUserId() == comment.UserId || User.IsInRole("Administrator") || coursesRepo.IsTeacherInCourse(comment.ProjectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")));
                
                return Json(new { CommentId = comment.Id, Comment = comment.Text, User = comment.ApplicationUser.Name, UserId = comment.UserId, Created = DateUtility.TimeAgoFromDateTime(comment.Created), CanDelete = canDeleteComment}, JsonRequestBehavior.AllowGet);
            }

            ViewBag.AllUsers = db.Users.ToList();
            return View("~/Views/ProjectTasks/Details.cshtml", comment.ProjectTask);
        }

        // POST: Comments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
