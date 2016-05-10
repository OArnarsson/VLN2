using Coder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coder.Models.Entity;
using Coder.Repositories;
using Microsoft.AspNet.Identity;

namespace Coder.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private CommentsRepository commentsRepo;
        private ProjectTasksRepository projectTasksRepo;
        private UsersRepository usersRepo;

        public CommentsController()
        {
            commentsRepo = new CommentsRepository(db);
            projectTasksRepo = new ProjectTasksRepository(db);
            usersRepo = new UsersRepository(db);
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

            ViewBag.AllUsers = db.Users.ToList();

            if (Request.IsAjaxRequest())
            {
                return Json(comment);
            }
            
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
