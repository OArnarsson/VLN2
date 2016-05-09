using Coder.Models;
using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Repositories
{
    public class SubmissionRepository
    {
        private readonly ApplicationDbContext db;

        public SubmissionRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        /*public Project GetProjectById(int? id)
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
        }*/

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}