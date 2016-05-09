using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coder.Models;
using Microsoft.AspNet.Identity;
using Coder.Models.ViewModels;
using Coder.Helpers;
using Coder.Repositories;

namespace Coder.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ProjectsRepository projectsRepository;
        private readonly CoursesRepository coursesRepository;
        private readonly UsersRepository usersRepository;

        public HomeController()
        {
            projectsRepository = new ProjectsRepository(db);
            coursesRepository = new CoursesRepository(db);
            usersRepository = new UsersRepository(db);
        }

        public ActionResult Index()
        {
            DashboardViewModel viewModel = new DashboardViewModel();

            viewModel.Courses = coursesRepository.GetCoursesForUser(User.Identity.GetUserId()).ToList();

            viewModel.Projects = (from x in (projectsRepository.GetProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).ToList()) orderby x.End ascending select x).Take(9).ToList();

            viewModel.Users = (User.IsInRole("Administrator")) ? usersRepository.GetAllUsers().ToList() : null;
           
            
            //if (viewModel.Projects.Count() < 10)
            //{
            //    //Filling up the list with inactive projects, ordered by most recent.
            //    var y = viewModel.Projects.ToList();
            //    y.AddRange((from x in db.Projects.ToList() where x.End < DateTime.Now orderby x.End descending select x).Take(9 - viewModel.Projects.Count()).ToList());
            //    viewModel.Projects = y.ToList();
            //}

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