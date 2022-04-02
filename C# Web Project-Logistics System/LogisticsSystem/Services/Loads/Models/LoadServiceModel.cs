
namespace LogisticsSystem.Services.Loads.Models
{
    public class LoadServiceModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string MainImageUrl { get; set; }

        public string PublishedOn { get; set; }

        public string DeletedOn { get; set; }

        public decimal Price { get; set; }

        public string Condition { get; set; }

        public bool IsPublic { get; set; }
    }
}
