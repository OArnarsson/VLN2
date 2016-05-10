using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Created { get; set; }
        public bool CanDelete { get; set; }
    }
}