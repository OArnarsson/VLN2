using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coder.Models.Entity;

namespace Coder.Models.ViewModels
{
    public class TaskViewModel
    {
        public ProjectTask Task { get; set; }
        public Submission BestSubmission { get; set; }
    }
}