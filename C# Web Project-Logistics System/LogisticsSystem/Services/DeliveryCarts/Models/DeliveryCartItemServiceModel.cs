
namespace LogisticsSystem.Services.DeliveryCarts.Models
{
    public class DeliveryCartItemServiceModel
    {
        public int Id { get; set; }

        public string LoadId { get; set; }

        public string LoadTitle { get; set; }

        public string LoadCondition { get; set; }

        public string LoadDelivery { get; set; }

        public byte LoadQuantity { get; init; }

        public byte Quantity { get; init; }

        public string ImageUrl { get; set; }

        public decimal Price { get; init; }

        public string TraderName { get; set; }

        public string TraderTelephoneNumber { get; set; }
    }
}
