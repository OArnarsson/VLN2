using Coder.Models;
using Coder.Models.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Coder.Repositories
{
    public class ProjectsRepository
    {
        private readonly ApplicationDbContext db;

        /*
        * Initialization.
        */
        public ProjectsRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        /*
        * Fetches all projects for the current user.
        */
        public List<Project> GetProjectsByUserId(string userId, bool isAdmin)
        {
            if (isAdmin)
            {
                return db.Projects.ToList();
            }

            return (from p in db.Projects
                    join c in db.Courses on p.CourseId equals c.Id
                    where c.UserCourses.Any(u => u.UserId == userId)
                    select p).ToList();
        }

        /*
        * Fetches all active projects for the current user.
        */
        public List<Project> GetActiveProjectsByUserId(string userId, bool isAdmin)
        {
            var projects = GetProjectsByUserId(userId, isAdmin);

            return (from p in projects
                   where p.Start < DateTime.Now && p.End > DateTime.Now
                   select p).ToList();
        }

        /*
        * Fetches all upcoming projects for the current user.
        */
        public List<Project> GetUpcomingProjectsByUserId(string userId, bool isAdmin)
        {
            var projects = GetProjectsByUserId(userId, isAdmin);

            return (from p in projects
                    where p.Start > DateTime.Now
                    select p).ToList();
        }

        /*
        * Fetches all expired projects for the current user.
        */
        public List<Project> GetExpiredProjectsByUserId(string userId, bool isAdmin)
        {
            var projects = GetProjectsByUserId(userId, isAdmin);

            return (from p in projects
                    where p.End < DateTime.Now
                    select p).ToList();
        }

        /*
        * Fetches a project with specific ID.
        */
        public Project GetProjectById(int? id)
        {
            return db.Projects.Find(id);
        }

        /*
        * Fetches all projects in a course with a specific ID.
        */
        public List<Project> GetAllProjectsByCourseId(int id)
        {
            return (from c in db.Projects
                   where c.CourseId == id
                   select c).ToList();
        }
        
        /*
        * Adds a project to the database.
        */
        public void AddProject(Project project)
        {
            db.Projects.Add(project);
            db.SaveChanges();
        }

        /*
        * Removes a project from the database.
        */
        public void RemoveProject(Project project)
        {
            db.Projects.Remove(project);
            db.SaveChanges();
        }

        /*
        * Updates a project in the database.
        */
        public void UpdateState(EntityState state, Project project)
        {
            db.Entry(project).State = state;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}