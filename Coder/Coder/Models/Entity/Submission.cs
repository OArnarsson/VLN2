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

        [ForeignKey("ProjectTask")]
        public int ProjectTaskId { get; set; }
        public virtual ProjectTask ProjectTask { get; set; }

        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public IEnumerable<SubmissionTestResult> SubmissionTestResults { get; set; }
    }
}
