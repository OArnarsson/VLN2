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

namespace Coder.Controllers
{
    public class SubmissionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private readonly SubmissionsRepository submissionsRepository;

        public SubmissionsController()
        {
            submissionsRepository = new SubmissionsRepository(db);
        }

        // GET: All submissions
        public ActionResult Index()
        {
            return View(db.Submissions.ToList());
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
