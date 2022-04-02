
namespace LogisticsSystem.Services.Reviews.Models
{
    public class ReviewListingServiceModel:IReviewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Rating { get; set; }

        public string Content { get; set; }

        public string PublishedOn { get; set; }

        public string LoadId { get; set; }

        public string LoadTitle { get; set; }

        public string LoadImage { get; set; }

        public int TotalComments { get; set; }
    }
}
