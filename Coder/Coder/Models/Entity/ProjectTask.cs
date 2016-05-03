using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class ProjectTask
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        IEnumerable<FileRequired> FilesRequired { get; set; }
        IEnumerable<TaskTest> TaskTests { get; set; }
        IEnumerable<Submission> Submissions { get; set; }
        IEnumerable<Comment> Comments { get; set; }

        public virtual Project Project { get; set; }
    }
}