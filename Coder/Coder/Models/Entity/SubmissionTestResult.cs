using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class SubmissionTestResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string ObtainedOutput { get; set; }
        public int Status { get; set; }

        public int SubmissionId { get; set; }
        [ForeignKey("SubmissionId")]
        public virtual Submission Submission { get; set; }
    }

    public enum TestResultStatus
    {
        Accepted = 1,
        CompileError = 2,
        MemoryError = 3,
        WrongOutput = 4
    }
}