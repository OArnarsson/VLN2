using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coder.Models.Entity
{
    public class ProjectTask
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Value must be a natural number")]
        public double Value { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Value must be a natural number")]
        public int MaxGroupSize { get; set; }
        
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public virtual ICollection<FileRequired> FilesRequired { get; set; }
        public virtual ICollection<TaskTest> TaskTests { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}