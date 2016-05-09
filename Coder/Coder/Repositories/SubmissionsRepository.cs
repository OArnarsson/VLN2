using Coder.Models;
using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Coder.Repositories
{
    public class SubmissionsRepository
    {
        private readonly ApplicationDbContext db;

        public SubmissionsRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        public void AddSubmission(Submission submission)
        {
            db.Submissions.Add(submission);
            db.SaveChanges();
        }

        public void UpdateState(EntityState state, Submission submission)
        {
            db.Entry(submission).State = state;
        }

        public void AddSubmissionTestResult(SubmissionTestResult result)
        {
            db.SubmissionTestResults.Add(result);
            db.SaveChanges();
        }

        public Submission GetSubmissionById(int? id)
        {
            return db.Submissions.Find(id);
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