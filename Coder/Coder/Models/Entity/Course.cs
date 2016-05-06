using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coder.Models.Entity
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        public string Description { get; set; }


        public string Title { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<UserCourse> UserCourses { get; set; }
    }
}