using System.ComponentModel.DataAnnotations;

namespace LogisticsSystem.Data.Models
{
    public class DeliveryCartItem
    {
        public int Id { get; set; }

        [Required]
        public string LoadId { get; set; }

        public virtual Load Load { get; set; }


        public string UserId { get; set; }

        public virtual User User { get; set; }

        public byte Quantity { get; set; }


        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
