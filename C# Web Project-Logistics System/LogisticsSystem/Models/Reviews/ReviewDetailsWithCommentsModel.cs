using LogisticsSystem.Services.Comments.Models;
using LogisticsSystem.Services.Reviews.Models;
using System.Collections.Generic;

namespace LogisticsSystem.Models.Reviews
{
    public class ReviewDetailsWithCommentsModel
    {
        public ReviewDetailsServiceModel Review { get; init; }

        public IEnumerable<CommentServiceModel> Comments { get; set; }
    }
}
