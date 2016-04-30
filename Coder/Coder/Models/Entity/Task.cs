using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public int ProjectId { get; set; }
        IEnumerable<FileRequired> FilesRequired { get; set; }
        IEnumerable<InputOutputPair> InputOutputPairs { get; set; }
    }
}