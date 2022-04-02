
namespace LogisticsSystem.Services.Loads.Models
{
    public class LoadDetailsServiceModel:LoadServiceModel
    {
        public byte Quantity { get; init; }

        public string Description { get; init; }

        public string Delivery { get; init; }

        public string DealerId { get; init; }

        public string DealerName { get; init; }

        public string SecondImageUrl { get; init; }

        public string ThirdImageUrl { get; init; }

        public string UserId { get; init; }

        public string KindId { get; init; }

        public string KindName { get; set; }

        public string SubKindId { get; init; }

        public string SubKindName { get; init; }
    }
}
