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

        public ProjectsRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

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

        public Project GetProjectById(int? id)
        {
            return db.Projects.Find(id);
        }

        public void AddProject(Project project)
        {
            db.Projects.Add(project);
            db.SaveChanges();
        }

        public void RemoveProject(Project project)
        {
            db.Projects.Remove(project);
            db.SaveChanges();
        }

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