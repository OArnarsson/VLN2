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
    public class ProjectTasksRepository
    {
        private readonly ApplicationDbContext db;

        /*
        * Initialization.
        */
        public ProjectTasksRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        /*
        * Fetches all the tasks of a given project.
        */
        public IEnumerable<ProjectTask> GetAllProjectTasks()
        {
            return db.ProjectTasks.Include(x => x.Project);
        }

        /*
        * Fetches all tasks in projects in courses where the user is enrolled in.
        */
        public IEnumerable<ProjectTask> GetAllProjectTasksForUserId(string userId)
        {
            return (from pt in db.ProjectTasks
                join p in db.Projects on pt.ProjectId equals p.Id
                join c in db.Courses on p.CourseId equals c.Id
                join uc in db.UserCourses on c.Id equals uc.CourseId
                where uc.UserId == userId
                select pt).ToList();
        }

        /*
        * Fetches a task with a specific ID.
        */
        public ProjectTask GetProjectTaskById(int? id)
        {
            return db.ProjectTasks.Find(id);
        }

        /*
        * Fetches all grades in projects in courses where the user is enrolled in.
        */
        public IEnumerable<GradeProjectTask> GetAllGradeProjectTasks()
        {
            return db.GradeProjectTasks.ToList();
        }

        /*
        * Fetches all grades in tasks in projects in courses where the user is enrolled in.
        */
        public IEnumerable<GradeProjectTask> GetAllGradeProjectTasksForTaskId(int taskId)
        {
            return db.GradeProjectTasks.Where(g => g.ProjectTaskId == taskId).ToList();
        }

        /*
        * Adds a task to the database.
        */
        public void AddProjectTask(ProjectTask projectTask)
        {
            db.ProjectTasks.Add(projectTask);
            db.SaveChanges();
        }

        /*
        * Adds information about the files required for a task to the database.
        */
        public void AddFilesRequired(FileRequired fileRequired)
        {
            db.FilesRequired.Add(fileRequired);
        }

        /*
        * Adds information about the tests for a task to the database.
        */
        public void AddTaskTests(TaskTest taskTest)
        {
            db.TaskTests.Add(taskTest);
        }

        /*
        * Removes a task from the database.
        */
        public void RemoveProjectTask(ProjectTask projectTask)
        {
            db.ProjectTasks.Remove(projectTask);
            db.SaveChanges();
        }

        /*
        * Removes all tests for a task from the database.
        */
        public void RemoveAllTaskTestsForProjectTask(ProjectTask projectTask)
        {
            foreach (var i in db.TaskTests.Where(x => x.ProjectTaskId == projectTask.Id))
            {
                db.TaskTests.Remove(i);
            }
        }

        /*
        * Removes all required files for a task from the database.
        */
        public void RemoveAllFilesRequiredForProjectTask(ProjectTask projectTask)
        {
            foreach (var f in db.FilesRequired.Where(i => i.ProjectTaskId == projectTask.Id))
            {
                db.FilesRequired.Remove(f);
            }
        }

        /*
        * Removes all grades for a task from the database.
        */
        public void RemoveAllGradesForProjectTask(ProjectTask projectTask)
        {
            foreach (var f in db.GradeProjectTasks.Where(i => i.ProjectTaskId == projectTask.Id))
            {
                db.GradeProjectTasks.Remove(f);
            }
        }

        /*
        * Updates the task in the database.
        */
        public void UpdateState(EntityState state, ProjectTask projectTask)
        {
            db.Entry(projectTask).State = state;
        }

        /*
        * Saves the changes to the database.
        */
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}