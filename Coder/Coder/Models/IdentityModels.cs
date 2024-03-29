﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Coder.Models.Entity;
using System.Collections.Generic;

namespace Coder.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public virtual ICollection<UserCourse> UserCourses { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<GradeProjectTask> GradeProjectTasks { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<SubmissionTestResult> SubmissionTestResults { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TaskTest> TaskTests { get; set; }
        public DbSet<FileRequired> FilesRequired { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<GradeProjectTask> GradeProjectTasks { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}