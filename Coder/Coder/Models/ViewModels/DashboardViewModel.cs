using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coder.Models.Entity;

namespace Coder.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<Course> Courses { get; set; }
        public List<Project> Projects { get; set; }
        public List<Project> ExpiredProjects { get; set; }
        public List<Project> OngoingProjects { get; set; }
        public List<Project> UpcomingProjects { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Submission> Submissions { get; set; }
    }
}