using LogisticsSystem.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Load;

namespace LogisticsSystem.Data.Models
{
    public class Load
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(TittleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public byte Quantity { get; set; }

        [Required]
        public LoadCondition LoadCondition { get; set; }

        [Required]
        public PersonType PersonType { get; set; }

        public DateTime PublishedOn { get; set; }

        public bool IsPublic { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string TraderId { get; set; }

        public virtual Trader Trader { get; set; }

        public int KindId { get; set; }

        public virtual Kind Kind { get; set; }
        
        public int SubKindId { get; set; }

        public virtual SubKind SubKind { get; set; }

        public virtual ICollection<LoadImage> Images { get; init; }
          = new HashSet<LoadImage>();

        public virtual ICollection<Review> Reviews { get; init; }
          = new HashSet<Review>();

        public virtual ICollection<Question> Questions { get; set; }
          = new HashSet<Question>();

        public virtual ICollection<Favourite> Favourites { get; set; }
          = new HashSet<Favourite>();

        public virtual ICollection<DeliveryCartItem> DeliveryCartItems { get; set; }
         = new HashSet<DeliveryCartItem>();

        public virtual ICollection<Report> Reports { get; set; }
         = new HashSet<Report>();
    }
}
