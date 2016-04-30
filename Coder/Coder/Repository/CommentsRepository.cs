using Coder.Models;
using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Repository
{
    public class CommentsRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Comment> getAllComments(int taskId)
        {
            return from comment in db.Comments.ToList()
                   where comment.ProjectTaskId == taskId
                   select comment;
        }
    }
}