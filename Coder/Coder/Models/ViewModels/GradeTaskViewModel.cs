using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.ViewModels
{
    public class GradeTaskViewModel
    {
        public IEnumerable<GradeProjectTask> GradeProjectTasks { get; set; }
        public IEnumerable<ApplicationUser> UsersWithSubmission { get; set; }
        public IEnumerable<ApplicationUser> UsersWithoutSubmission { get; set; }
    }
}