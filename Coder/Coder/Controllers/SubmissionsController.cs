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
using Coder.Helpers;

namespace Coder.Controllers
{
    public class SubmissionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: All submissions
        public ActionResult Index()
        {
            return View(db.Submissions.ToList());
        }
        /*
        // GET: All submissions by userId
        public ActionResult User(string userId)
        {
            IEnumerable<Submission> submissions = from s in db.Submissions.ToList()
                                                  where s.ApplicationUsers.Any(u => u.Id == userId)
                                                  select s;
            return View(submissions);
        }

        // GET: All submissions by taskId
        public ActionResult Task(int taskId)
        {
            IEnumerable<Submission> submissions = from s in db.Submissions.ToList()
                                                  where s.ProjectTaskId == taskId
                                                  select s;

            return View(submissions);
        }

        // GET: All submissions by projectId
        public ActionResult Project(int projectId)
        {
            IEnumerable<Submission> submissions = from submission in db.Submissions
                                                  join t in db.ProjectTasks on submission.ProjectTaskId equals t.Id
                                                  where t.Id == projectId
                                                  select submission;
                                                    
            return View(db.Submissions.ToList());
        }
        */
    }
}
