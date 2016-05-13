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
            var viewModel = new DashboardViewModel();

            var ongoingProjects = projectsRepository.GetActiveProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).ToList();
            var upcomingProjects = projectsRepository.GetUpcomingProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).Take(5).ToList();

            var ongoingProjectsViewModel = new List<ProjectViewModel>();
            foreach (var ogP in ongoingProjects)
            {
                ongoingProjectsViewModel.Add(new ProjectViewModel {Project = ogP, Grade = 0, Value = 0 });
            }

            var upcomingProjectsViewModel = new List<ProjectViewModel>();
            foreach (var ucP in upcomingProjects)
            {
                upcomingProjectsViewModel.Add(new ProjectViewModel { Project = ucP, Grade = 0, Value = 0 });
            }

            viewModel.Courses = coursesRepository.GetCoursesForUser(User.Identity.GetUserId()).ToList();
            viewModel.Submissions = submissionsRepository.GetSubmissionsForUserId(User.Identity.GetUserId()).ToList();
            viewModel.UpcomingProjects = upcomingProjectsViewModel;
            viewModel.OngoingProjects = ongoingProjectsViewModel;
            var projects = projectsRepository.GetExpiredProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).Take(5).ToList();
            viewModel.ExpiredProjects = GetProjectsWithGrade(projects);
            viewModel.Users = (User.IsInRole("Administrator")) ? usersRepository.GetAllUsers().ToList() : null;

            return View(viewModel);
        }

        public List<ProjectViewModel> GetProjectsWithGrade(List<Project> projects)
        {
            var list = new List<ProjectViewModel>();

            foreach (var project in projects)
            {
                list.Add(new ProjectViewModel { Project = project, Grade = GetGrade(project) });
            }

            return list;
        }

        public double GetGrade(Project project)
        {
            double totalGrade = 0;
            double totalGrades = 0;
            double totalValue = 0;
            foreach (var task in project.ProjectTasks)
            {
                var gradeProjectTask = task.GradeProjectTasks.Where(g => g.UserId == User.Identity.GetUserId()).FirstOrDefault();
                if (gradeProjectTask != null)
                {
                    totalGrade += task.GradeProjectTasks.Where(g => g.UserId == User.Identity.GetUserId()).FirstOrDefault().Grade * task.Value;
                    totalValue += task.Value;
                    totalGrades++;
                }
            }
            if (totalGrades == project.ProjectTasks.Count && (project.ProjectTasks.Count > 0))
            {
                return Math.Round(totalGrade / totalValue, 2);
            }
            return 0;
        }

        public ActionResult Boxes()
        {
            var viewModel = new DashboardViewModel();

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
            var projects = projectsRepository.GetExpiredProjectsByUserId(User.Identity.GetUserId(), User.IsInRole("Administrator")).Take(5).ToList();
            viewModel.ExpiredProjects = GetProjectsWithGrade(projects);
            viewModel.Submissions = submissionsRepository.GetSubmissionsForUserId(User.Identity.GetUserId()).ToList();
            viewModel.Users = (User.IsInRole("Administrator")) ? usersRepository.GetAllUsers().ToList() : null;

            return View(viewModel);
        }

        public List<ProjectViewModel> GetProjectsWithValue(List<Project> projects)
        {
            var list = new List<ProjectViewModel>();

            foreach (var project in projects)
            {
                list.Add(new ProjectViewModel {Project = project, Value = GetValue(project)});
            }
            return list;
        }

        public double GetValue(Project projects)
        {
            double currentValue = 0;
            double totalValue = 0;
            foreach (var task in projects.ProjectTasks)
            {
                var bestSubmisson = submissionsRepository.GetBestUserSubmissionForTask(task.Id, User.Identity.GetUserId());
                if (bestSubmisson != null && bestSubmisson.Status == TestResultStatus.Accepted)
                {
                    currentValue += task.Value;
                }
                totalValue += task.Value;
            }

            if (totalValue == 0)
            {
                return 0;
            }

            return Math.Round((currentValue/totalValue)*100);
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
    }
}