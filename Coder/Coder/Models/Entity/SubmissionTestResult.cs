using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class SubmissionTestResult
    {
        public int Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string ObtainedOutput { get; set; }
        public int Status { get; set; }

        [ForeignKey("Submission")]
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
    }
}