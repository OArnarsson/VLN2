using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoderUi.DummyModels;

namespace CoderUi.DummyModels
{
    public class CoursesAndAssignments
    {
        public int id { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }        
        public List<DummyUsers> allUsers { get; set;}
        public List<Assignment> assignments { get; set; }

    }
   
}