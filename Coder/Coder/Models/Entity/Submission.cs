using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class Submission
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int State { get; set; }
        public int TaskId { get; set; }
        public string Output { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
