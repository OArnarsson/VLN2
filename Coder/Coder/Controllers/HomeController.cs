using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coder.Models;
using Microsoft.AspNet.Identity;
using Coder.Models.ViewModels;
using Coder.Helpers;

namespace Coder.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            DashboardViewModel viewModel = new DashboardViewModel()
            {
                Courses = db.Courses.ToList(),
                Users = db.Users.ToList(),
                //Tries to take only projects that are still active, ordered by time remaining. 
                Projects = (from x in db.Projects.ToList() where x.End >= DateTime.Now orderby x.End select x).Take(9).ToList()
            };

            
            if (viewModel.Projects.Count() < 10)
            {
                //Filling up the list with inactive projects, ordered by most recent.
                var y = viewModel.Projects.ToList();
                y.AddRange((from x in db.Projects.ToList() where x.End < DateTime.Now orderby x.End descending select x).Take(9 - viewModel.Projects.Count()).ToList());
                viewModel.Projects = y.ToList();
            }

            return View(viewModel);
        }

        public int GetUsersCount()
        {
            return db.Users.Count();
        }

        public int GetCoursesCount()
        {
            return db.Courses.Count();
        }

        public int GetProjectsCount()
        {
            return db.Projects.Count();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}