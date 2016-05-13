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

        /*
        * Initialization.
        */
        public SubmissionsRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
            coursesRepository = new CoursesRepository(db);
        }

        /*
        * Fetches all submissions for a task.
        */
        public IEnumerable<Submission> GetAllSubmissions()
        {
            return db.Submissions.ToList();
        }

        /*
        * Fetches all submissions from all users.
        */
        public IEnumerable<Submission> GetSubmissionsForUserId(string userId)
        {
            return (from s in db.Submissions
                from u in s.ApplicationUsers
                where u.Id == userId
                orderby s.Id descending 
                select s).ToList();
        }

        /*
        * Fetches all submissions in courses where the user is a teacher
        */
        public IEnumerable<Submission> GetSubmissionsForTeacherId(string userId)
        {
            return GetSubmissionsForCourses(coursesRepository.GetCoursesForTeacherWithTeacherRole(userId));
        }

        /*
        * Fetches all submissions in courses where the user is an assistant teacher
        */
        public IEnumerable<Submission> GetSubmissionsForAssistantTeacherId(string userId)
        {
            return GetSubmissionsForCourses(coursesRepository.GetCoursesForTeacherWithAssistantTeacherRole(userId));
        }

        /*
        * Fetches all submissions in a course.
        */
        public IEnumerable<Submission> GetSubmissionsForCourses(IEnumerable<Course> courses)
        {
            return (from c in courses
                join p in db.Projects on c.Id equals p.CourseId
                join pt in db.ProjectTasks on p.Id equals pt.ProjectId
                join s in db.Submissions on pt.Id equals s.ProjectTaskId
                select s).ToList();
        }

        /*
        * Fetches all submissions for a task with a specific ID.
        */
        public IEnumerable<Submission> GetSubmissionsForProjectTaskId(int projectTaskId)
        {
            return db.Submissions.Where(s => s.ProjectTaskId == projectTaskId).ToList();
        }

        /*
        * Adds a submission to the database.
        */
        public void AddSubmission(Submission submission)
        {
            db.Submissions.Add(submission);
            db.SaveChanges();
        }

        /*
        * Updates a submission in the database.
        */
        public void UpdateState(EntityState state, Submission submission)
        {
            db.Entry(submission).State = state;
        }

        /*
        * Adds a submission test result to the database.
        */
        public void AddSubmissionTestResult(SubmissionTestResult result)
        {
            db.SubmissionTestResults.Add(result);
            db.SaveChanges();
        }

        /*
        * Fetches a submission with a specific ID.
        */
        public Submission GetSubmissionById(int? id)
        {
            return db.Submissions.Find(id);
        }

        /*
        * Fetches the submission with most tests passed/newest from the user.
        */
        public Submission GetBestUserSubmissionForTask(int taskId, string userId)
        {
            var submissions = db.Submissions.Where(i => i.ApplicationUsers.Any(j => j.Id == userId) && i.ProjectTaskId == taskId);
            return (from s in submissions
                orderby s.SubmissionTestResults.Count(i => i.Status == TestResultStatus.Accepted) descending, s.Created descending
                select s).FirstOrDefault();
        }

        /*
        * Saves changes to the database.
        */
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}