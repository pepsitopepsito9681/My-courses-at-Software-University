
namespace LogisticsSystem.Services.Favourites.Models
{
    public class FavouriteServiceModel
    {
        public int Id { get; set; }

        public string LoadId { get; set; }

        public string LoadTitle { get; set; }

        public string LoadCondition { get; set; }

        public int ReviewsCount { get; set; }

        public int QuestionsCount { get; set; }

        public string ImageUrl { get; set; }

        public byte Quantity { get; init; }

        public string Price { get; init; }

        public string LoadDelivery { get; set; }
    }
}
