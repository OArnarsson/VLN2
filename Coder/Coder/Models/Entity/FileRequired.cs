using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class FileRequired
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TaskId { get; set; }
    }
}