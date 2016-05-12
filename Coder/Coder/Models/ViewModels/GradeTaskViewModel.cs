using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.ViewModels
{
    public class GradeTaskViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Submission Submission { get; set; }
        public GradeProjectTask GradeProjectTask { get; set; }
    }
}