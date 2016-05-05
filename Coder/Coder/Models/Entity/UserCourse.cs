using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class UserCourse
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Key, Column(Order = 1)]
        public int CourseId { get; set; }

        public CoderRole CoderRole { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Course Course { get; set; }
    }

    public enum CoderRole
    {
        Student = 1,
        TeachingAssistant = 2,
        Teacher = 3
    }
}