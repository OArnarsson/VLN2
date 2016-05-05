using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class Submission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Created { get; set; }

        public int ProjectTaskId { get; set; }
        [ForeignKey("ProjectTaskId")]
        public virtual ProjectTask ProjectTask { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<SubmissionTestResult> SubmissionTestResults { get; set; }
    }
}
