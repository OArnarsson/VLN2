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
using Coder.Helpers;
using Coder.Repositories;
using Microsoft.AspNet.Identity;

namespace Coder.Controllers
{
    public class SubmissionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private readonly SubmissionsRepository submissionsRepository;
        private readonly CoursesRepository coursesRepository;

        public SubmissionsController()
        {
            submissionsRepository = new SubmissionsRepository(db);
            coursesRepository = new CoursesRepository(db);
        }

        // GET: All submissions
        public ActionResult Index()
        {
            // Check if teacher in course, return all submissions for that course
            // Check if admin, return all submissions

            ViewBag.IsTeacher = coursesRepository.IsTeacherInAnyCourse(User.Identity.GetUserId(), User.IsInRole("Administrator"));
            return View(submissionsRepository.GetSubmissionsForUserId(User.Identity.GetUserId()));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request!");
            }

            Submission submission = submissionsRepository.GetSubmissionById(id.Value);

            if (submission == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Not found!");
            }

            return View();
        }
    }
}
