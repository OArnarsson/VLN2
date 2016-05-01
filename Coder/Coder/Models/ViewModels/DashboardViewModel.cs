using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coder.Models.Entity;

namespace Coder.Models.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set;  }
    }
}