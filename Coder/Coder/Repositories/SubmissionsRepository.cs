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
        private readonly UsersRepository usersRepository;
        private readonly CoursesRepository coursesRepository;

        public SubmissionsRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
            coursesRepository = new CoursesRepository(db);
        }

        public IEnumerable<Submission> GetAllSubmissions()
        {
            return db.Submissions.ToList();
        }

        // Gets all submissions for user, no matter the role he's in
        public IEnumerable<Submission> GetSubmissionsForUserId(string userId)
        {
            return (from s in db.Submissions
                    from u in s.ApplicationUsers
                    where u.Id == userId
                    select s).ToList();
        }

        // Gets all submissions in courses where the user is a teacher
        public IEnumerable<Submission> GetSubmissionsForTeacherId(string userId)
        {
            return GetSubmissionsForCourses(coursesRepository.GetCoursesForTeacherWithTeacherRole(userId));
        }

        public IEnumerable<Submission> GetSubmissionsForCourses(IEnumerable<Course> courses)
        {
            return (from c in courses
                    join p in db.Projects on c.Id equals p.CourseId
                    join pt in db.ProjectTasks on p.Id equals pt.ProjectId
                    join s in db.Submissions on pt.Id equals s.ProjectTaskId
                    select s).ToList();
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