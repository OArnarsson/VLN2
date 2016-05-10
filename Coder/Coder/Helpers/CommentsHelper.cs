using Coder.Models;
using Coder.Models.Entity;
using Coder.Models.ViewModels;
using Coder.Repositories;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace Coder.Helpers
{
    public class CommentsHelper
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly CoursesRepository coursesRepo;

        public CommentsHelper()
        {
            coursesRepo = new CoursesRepository(db);
        }

        public bool CanDelete(Comment comment, string userId, bool isAdmin)
        {
            return (isAdmin || userId == comment.UserId || coursesRepo.IsTeacherInCourse(comment.ProjectTask.Project.CourseId, userId, isAdmin));
        }

        public IEnumerable<CommentViewModel> CommentViewModelsFromComments(IEnumerable<Comment> comments, bool isAdmin, string userId)
        {
            List<CommentViewModel> commentsViewModel = new List<CommentViewModel>();
            foreach (var c in comments)
            {
                CommentViewModel commentVM = new CommentViewModel()
                {
                    Id = c.Id,
                    Name = c.ApplicationUser.Name,
                    Text = c.Text,
                    Created = DateUtility.TimeAgoFromDateTime(c.Created),
                    CanDelete = CanDelete(c, userId, isAdmin)
                };
                commentsViewModel.Add(commentVM);
            }

            return commentsViewModel;
        }
    }
}