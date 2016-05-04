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
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        // TODO: Check security
        //[Required]
        //public string Password { get; set; }

        public List<Course> Courses { get; set; }

        public List<UserCourse> UserCourses { get; set; }

        public UserViewModel()
        {

        }

        public UserViewModel(ApplicationUser user)
        {
            UserId = user.Id;
            Name = user.Name;
            Email = user.Email;
        }
    }
}