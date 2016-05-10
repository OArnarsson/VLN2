﻿using Coder.Models;
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
        private readonly ApplicationDbContext db;

        public CommentsRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        public IEnumerable<Comment> getCommentsForProjectTaskId(int projectTaskId)
        {
            return (from comment in db.Comments.ToList()
                   where comment.ProjectTaskId == projectTaskId
                   orderby comment.Created descending
                   select comment).ToList();
        }

        public void AddComment(Comment comment)
        {
            db.Comments.Add(comment);
            db.SaveChanges();
        }
    }
}