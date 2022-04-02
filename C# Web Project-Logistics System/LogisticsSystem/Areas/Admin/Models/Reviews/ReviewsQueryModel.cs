using LogisticsSystem.Services.Reviews.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogisticsSystem.Areas.Admin.Models.Reviews
{
    public class ReviewsQueryModel
    {
        public const int ReviewsPerPage = 9;

        public int CurrentPage { get; set; } = 1;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int TotalReviews { get; set; }

        public IEnumerable<ReviewServiceModel> Reviews { get; set; }
    }
}
