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
        private readonly ApplicationDbContext db;

        /*
        * Initialization.
        */
        public CommentsRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        /*
        * Fetches the comments for the task at hand.
        */
        public IEnumerable<Comment> GetCommentsForProjectTaskId(int projectTaskId)
        {
            return (from comment in db.Comments.ToList()
                where comment.ProjectTaskId == projectTaskId
                orderby comment.Created ascending
                select comment).ToList();
        }


        /*
        * Fetches a single comment with specified ID.
        */
        public Comment GetCommentWithId(int id)
        {
            return db.Comments.Find(id);
        }


        /*
        * Adds a comment to the database.
        */
        public void AddComment(Comment comment)
        {
            db.Comments.Add(comment);
            db.SaveChanges();
        }


        /*
        * Deletes a comment with a specified ID.
        */
        public void RemoveCommentWithId(int id)
        {
            var comment = GetCommentWithId(id);

            if (comment != null)
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
        }
    }
}