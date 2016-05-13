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
using Coder.Models.Entity;

namespace Coder.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ProjectsRepository projectsRepository;
        private readonly CoursesRepository coursesRepository;
        private readonly UsersRepository usersRepository;
        private readonly SubmissionsRepository submissionsRepository;

        public HomeController()
        {
            projectsRepository = new ProjectsRepository(db);
            coursesRepository = new CoursesRepository(db);
            usersRepository = new UsersRepository(db);
            submissionsRepository = new SubmissionsRepository(db);
        }

        public ActionResult Index()
        {
            DashboardViewModel viewModel = new DashboardViewModel();

            viewModel.Courses = coursesRepository.GetCoursesForUser(User.Identity.GetUserId()).ToList();
            viewModel.OngoingProjects = projectsRepository.GetActiveProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).ToList();
            viewModel.UpcomingProjects = projectsRepository.GetUpcomingProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).Take(5).ToList();
            viewModel.ExpiredProjects = projectsRepository.GetExpiredProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).Take(5).ToList();
            viewModel.Submissions = submissionsRepository.GetSubmissionsForUserId(User.Identity.GetUserId()).ToList();
            viewModel.Users = (User.IsInRole("Administrator")) ? usersRepository.GetAllUsers().ToList() : null;                           

            return View(viewModel);
        }

        public ActionResult Boxes()
        {
            DashboardViewModel viewModel = new DashboardViewModel();

            viewModel.Courses = coursesRepository.GetCoursesForUser(User.Identity.GetUserId()).ToList();

            // Get active projects
            var activeProjects = projectsRepository.GetActiveProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator"));

            // if active projects are less than nine, get 9-active of projects with start date > Today, order by date
            var notStartedProjects = new List<Project>();
            if (activeProjects.Count < 9)
            {
                notStartedProjects = projectsRepository.GetUpcomingProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator"));
            }

            activeProjects.AddRange(notStartedProjects);
            viewModel.Projects = GetProjectsWithValue(activeProjects);

            // viewModel.Projects = (from x in (projectsRepository.GetProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).ToList()) orderby x.Start ascending select x).Take(9).ToList();
            viewModel.ExpiredProjects = projectsRepository.GetExpiredProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).Take(5).ToList();
            viewModel.Submissions = submissionsRepository.GetSubmissionsForUserId(User.Identity.GetUserId()).ToList();
            viewModel.Users = (User.IsInRole("Administrator")) ? usersRepository.GetAllUsers().ToList() : null;

            return View(viewModel);
        }

        public List<ProjectViewModel> GetProjectsWithValue(List<Project> projects)
        {
            List<ProjectViewModel> List = new List<ProjectViewModel>();

            foreach(var project in projects)
            {
                List.Add(new ProjectViewModel { project = project, value = GetValue(project) });
            }
            return List;
        }

        public double GetValue(Project projects)
        {          
            double currentValue = 0;
            double TotalValue = 0;
            foreach (var task in projects.ProjectTasks)
            {
                var gradeProjectTask = task.GradeProjectTasks.Where(g => g.UserId == User.Identity.GetUserId()).FirstOrDefault();
                if (gradeProjectTask != null)
                {
                    currentValue += task.Value;
                }
                TotalValue += task.Value;
            }
            
            if(TotalValue == 0)
            {
                return 0;
            }        
                
            else return Math.Round((currentValue / TotalValue)*100);
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