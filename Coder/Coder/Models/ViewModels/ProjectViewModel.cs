using Coder.Models.Entity;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Coder.Models.ViewModels
{
    public class ProjectViewModel
    {
       public Project project { get; set; }

        public double value { get; set; }
    }
}
