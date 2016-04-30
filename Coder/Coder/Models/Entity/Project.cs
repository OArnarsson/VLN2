using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Value { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}