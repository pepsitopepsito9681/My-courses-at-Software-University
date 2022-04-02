using System.Collections.Generic;

namespace LogisticsSystem.Services.Reviews.Models
{
    public class ReviewQueryModel
    {
        public int CurrentPage { get; init; }

        public int ReviewsPerPage { get; init; }

        public int TotalReviews { get; init; }

        public IEnumerable<ReviewServiceModel> Reviews { get; init; }
    }
}
