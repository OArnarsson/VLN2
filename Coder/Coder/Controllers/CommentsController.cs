using Coder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coder.Models.Entity;
using Coder.Repositories;

namespace Coder.Controllers
{
    public class CommentsController : Controller
    {
        private CommentsRepository commentsRepo = new CommentsRepository();

        // GET: Comments
        public ActionResult Index(int taskId)
        {
            return View(commentsRepo.getAllComments(taskId));
        }

        // POST: Comments/Create
        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            return View();
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
