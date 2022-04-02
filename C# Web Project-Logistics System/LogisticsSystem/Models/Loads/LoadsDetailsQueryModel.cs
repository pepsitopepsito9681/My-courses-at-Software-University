using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Services.Loads.Models;
using LogisticsSystem.Services.Questions.Models;
using LogisticsSystem.Services.Reviews.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogisticsSystem.Models.Loads
{
    public class LoadsDetailsQueryModel
    {
        public LoadDetailsServiceModel Load { get; set; }

        public IEnumerable<LoadServiceModel> SimilarLoads { get; set; }

        public const int ReviewsPerPage = 9;

        public string ReviewsSearchTerm { get; set; }

        [Display(Name = "Filter")]
        public ReviewKind? ReviewFiltering { get; init; }

        public int ReviewsCurrentPage { get; set; } = 1;

        public int TotalReviews { get; set; }


        public const int QuestionsPerPage = 9;

        public int QuestionsCurrentPage { get; set; } = 1;

        public int TotalQuestions { get; set; }


        public ReviewsLoadStatisticsServiceModel LoadReviewsStatistics { get; set; }

        public IEnumerable<ReviewServiceModel> Reviews { get; set; }

        public IEnumerable<QuestionServiceModel> Questions { get; set; }
    }
}
