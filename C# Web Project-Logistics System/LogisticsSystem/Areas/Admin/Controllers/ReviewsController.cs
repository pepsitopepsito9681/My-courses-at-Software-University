using LogisticsSystem.Areas.Admin.Models.Reviews;
using LogisticsSystem.Services.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Areas.Admin.Controllers
{
    public class ReviewsController : AdminController
    {
        private readonly IReviewsService reviews;

        public ReviewsController(IReviewsService reviews)
        => this.reviews = reviews;

        public IActionResult All([FromQuery] ReviewsQueryModel query)
        {
            var queryResult = this.reviews.All(
         query.SearchTerm,
         query.CurrentPage,
         ReviewsQueryModel.ReviewsPerPage,
         IsPublicOnly: false);

            query.Reviews = queryResult.Reviews;
            query.TotalReviews = queryResult.TotalReviews;

            return this.View(query);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.reviews.ChangeVisibility(id);

            return RedirectToAction(nameof(All));
        }
    }
}
