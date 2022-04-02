using LogisticsSystem.Services.Loads.Models;
using LogisticsSystem.Services.Questions.Models;
using LogisticsSystem.Services.Reviews.Models;
using System.Collections.Generic;

namespace LogisticsSystem.Models.Loads
{
    public class LoadDetailsModel
    {
        public LoadDetailsServiceModel Load { get; init; }

        public IEnumerable<LoadServiceModel> SimilarLoads { get; init; }

        public ReviewsLoadStatisticsServiceModel ProductReviewsStatistics { get; set; }

        public IEnumerable<ReviewServiceModel> Reviews { get; set; }

        public IEnumerable<QuestionServiceModel> Questions { get; set; }
    }
}
