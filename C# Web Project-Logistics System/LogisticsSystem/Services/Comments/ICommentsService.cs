using LogisticsSystem.Services.Comments.Models;
using System.Collections.Generic;

namespace LogisticsSystem.Services.Comments
{
    public interface ICommentsService
    {
        void Create(
            int reviewId,
            string userId,
            string content,
            bool IsPublic = false);

        IEnumerable<CommentServiceModel> CommentsOfReview(int reviewId);

        CommentQueryModel All(
            string searchTerm = null,
            int currentPage = 1,
            int commentsPerPage = int.MaxValue,
            bool IsPublicOnly = true);

        void ChangeVisibility(int id);

        bool Delete(int id);
    }
}
