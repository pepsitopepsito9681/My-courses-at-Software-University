using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Order;

namespace LogisticsSystem.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(AddressMаxLength)]
        public string Address { get; set; }

        [Required]
        [MaxLength(CityMаxLength)]
        public string City { get; set; }

        [Required]
        [MaxLength(StateMаxLength)]
        public string State { get; set; }

        [Required]
        [MaxLength(PostCodeMaxLength)]
        public string PostCode { get; set; }

        [Required]
        [MaxLength(TelephoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        public DateTime OrderedOn { get; set; }

        public bool IsAccomplished { get; set; }

        public virtual ICollection<DeliveryCartItem> DeliveryCart { get; set; }
        = new HashSet<DeliveryCartItem>();

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
