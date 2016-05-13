using System;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coder.Models.Entity
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        [GreaterThan("Start")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Value must be a natural number")]
        public double Value { get; set; }

        public int CourseId { get; set; }

        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}