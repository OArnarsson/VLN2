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

        public ProjectTasksRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        public IEnumerable<ProjectTask> GetAllProjectTasks()
        {
            return db.ProjectTasks.Include(x => x.Project);
        }

        public IEnumerable<ProjectTask> GetAllProjectTasksForUserId(string userId)
        {
            return (from pt in db.ProjectTasks
                    join p in db.Projects on pt.ProjectId equals p.Id
                    join c in db.Courses on p.CourseId equals c.Id
                    join uc in db.UserCourses on c.Id equals uc.CourseId
                    where uc.UserId == userId
                    select pt).ToList();
        }

        public ProjectTask GetProjectTaskById(int? id)
        {
            return db.ProjectTasks.Find(id);
        }

        public IEnumerable<GradeProjectTask> GetAllGradeProjectTasks()
        {
            return db.GradeProjectTasks.ToList();
        }

        public IEnumerable<GradeProjectTask> GetAllGradeProjectTasksForTaskId(int taskId)
        {
            return db.GradeProjectTasks.Where(g => g.ProjectTaskId == taskId).ToList();
        }

        public void AddProjectTask(ProjectTask projectTask)
        {
            db.ProjectTasks.Add(projectTask);
            db.SaveChanges();
        }

        public void AddFilesRequired(FileRequired fileRequired)
        {
            db.FilesRequired.Add(fileRequired);
        }
        
        public void AddTaskTests(TaskTest taskTest)
        {
            db.TaskTests.Add(taskTest);
        }

        public void RemoveProjectTask(ProjectTask projectTask)
        {
            db.ProjectTasks.Remove(projectTask);
            db.SaveChanges();
        }

        public void RemoveAllTaskTestsForProjectTask(ProjectTask projectTask)
        {
            foreach (var i in db.TaskTests.Where(x => x.ProjectTaskId == projectTask.Id))
            {
                db.TaskTests.Remove(i);
            }
        }

        public void RemoveAllFilesRequiredForProjectTask(ProjectTask projectTask)
        {
            foreach (var f in db.FilesRequired.Where(i => i.ProjectTaskId == projectTask.Id))
            {
                db.FilesRequired.Remove(f);
            }
        }

        public void UpdateState(EntityState state, ProjectTask projectTask)
        {
            db.Entry(projectTask).State = state;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}