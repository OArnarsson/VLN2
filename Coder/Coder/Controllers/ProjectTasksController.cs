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

namespace Coder.Controllers
{
    [ValidateInput(false)]
    public class ProjectTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectTasks
        public ActionResult Index()
        {
            var projectTasks = db.ProjectTasks.Include(p => p.Project);
            return View(projectTasks.ToList());
        }

        // GET: ProjectTasks/Details/5
        [SiteMapTitle("title")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // GET: ProjectTasks/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Value,ProjectId")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTasks.Add(projectTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectTask.ProjectId);
            return View(projectTask);
        }

        // GET: ProjectTasks/Edit/5
        [SiteMapTitle("title")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectTask.ProjectId);
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectTask projectTask, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTask).State = EntityState.Modified;
                db.SaveChanges();


                foreach (var i in db.TaskTests.Where(x => x.ProjectTaskId == projectTask.Id))
                {
                    db.TaskTests.Remove(i);
                }

                foreach (var f in db.FilesRequired.Where(i => i.ProjectTaskId == projectTask.Id))
                {
                    db.FilesRequired.Remove(f);
                }

                foreach (var file in form.GetValue("files").AttemptedValue.Split(','))
                {
                    if (String.IsNullOrEmpty(file.Trim()))
                    {
                        continue;
                    }
                    db.FilesRequired.Add(new FileRequired { Name = file.Trim(), ProjectTaskId = projectTask.Id });
                }

                for (int i = 0; i < form.Count; i++)
                {
                    var key = form.Keys[i];

                    if (key.StartsWith("test") && key.EndsWith("output"))
                    {
                        var counter = key.Split('_')[1];
                        string input = form.GetValue("test_" + counter + "_input").AttemptedValue.Replace("&quot;", "\"");
                        string output = form.GetValue("test_" + counter + "_output").AttemptedValue.Replace("&quot;", "\"");
                        db.TaskTests.Add(new TaskTest { Input = input, Output = output, ProjectTaskId = projectTask.Id });
                    }
                }

                db.SaveChanges();

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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            db.ProjectTasks.Remove(projectTask);
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

        public ActionResult SaveUploadedFile(int Id)
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                bool fileMatches = false;
                foreach (string fileName in Request.Files)
                {
                    // TODO: Check if fileName is in FilesRequired for this ProjectTask
                    ProjectTask task = db.ProjectTasks.FirstOrDefault(x => x.Id == Id);
                    
                    foreach(FileRequired fileRequired in task.FilesRequired)
                    {
                        if (fileRequired.Name == fileName)
                        {
                            fileMatches = true;
                        }
                    }

                    if (!task.FilesRequired.Any(i => i.Name == fileName)) {
                        Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        Response.ContentType = "text/plain";
                        Response.Write("Unable to connect to database on ");
                        return Json(new { error = "File not allowed " + fileName });
                    }

                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}Uploads\\Submissions", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);
                    }
                }
            }
            catch (Exception ex)
            {
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
    }
}
