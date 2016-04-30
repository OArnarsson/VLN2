using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class InputOutputPair
    {
        public int Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public int TaskId { get; set; }
    }
}