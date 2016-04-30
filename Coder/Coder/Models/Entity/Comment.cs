using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
    }
}