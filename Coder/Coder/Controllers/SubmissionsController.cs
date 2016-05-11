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
using MvcSiteMapProvider.Web.Mvc.Filters;

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
        [SiteMapTitle("title")]
        public ActionResult Index()
        {
            ViewBag.IsTeacher = coursesRepository.IsTeacherInAnyCourse(User.Identity.GetUserId(), User.IsInRole("Administrator"));

            if (User.IsInRole("Administrator"))
            {
                return View(submissionsRepository.GetAllSubmissions());
            }

            if (coursesRepository.IsTeacherInAnyCourse(User.Identity.GetUserId(), User.IsInRole("Administrator")))
            {
                var submissionsAsTeacher = submissionsRepository.GetSubmissionsForTeacherId(User.Identity.GetUserId());
                var submissionsAsStudent = submissionsRepository.GetSubmissionsForUserId(User.Identity.GetUserId());

                return View(submissionsAsStudent.Concat(submissionsAsTeacher));
            }
            
            return View(submissionsRepository.GetSubmissionsForUserId(User.Identity.GetUserId()));
        }

        [SiteMapTitle("title")]
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

            if (!submission.ApplicationUsers.Any(u => u.Id == User.Identity.GetUserId()) && !coursesRepository.IsTeacherInCourse(submission.ProjectTask.Project.CourseId, User.Identity.GetUserId(), User.IsInRole("Administrator")) && !User.IsInRole("Administrator"))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden!");
            }

            SubmissionsHelper helper = new SubmissionsHelper(db);
            ViewBag.submissionFolder = helper.getSubmissionFolder(submission);

            return View(submission);
        }
    }
}
