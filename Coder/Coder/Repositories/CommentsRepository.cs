using Coder.Models;
using Coder.Models.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Coder.Repositories
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