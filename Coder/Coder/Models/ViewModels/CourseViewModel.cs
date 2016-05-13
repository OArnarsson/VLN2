using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coder.Models.ViewModels
{
    public class CourseViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public int CourseId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }
        public List<UserCourse> UserCourses { get; set; }

        public CourseViewModel()
        {
        }

        public CourseViewModel(Course course)
        {
            CourseId = course.Id;
            Name = course.Name;
            Description = course.Description;
            Title = course.Title;
            Start = course.Start;
            End = course.End;
        }
    }
}