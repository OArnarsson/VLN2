using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public List<TaskViewModel> Tasks { get; set; }
    }
}