using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Coder.Models.ViewModels
{
    public class UserViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Course> Courses { get; set; }
        public ApplicationUser CurrentUser { get; set; }

        [Display(Name = "User role")]
        public IEnumerable<UserCourse> UserCourses { get; set; }
    }
}