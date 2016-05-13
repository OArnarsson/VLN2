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

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool Admin { get; set; }

        public List<Course> Courses { get; set; }

        public List<UserCourse> UserCourses { get; set; }

        public IEnumerable<Submission> Submissions { get; set; }

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