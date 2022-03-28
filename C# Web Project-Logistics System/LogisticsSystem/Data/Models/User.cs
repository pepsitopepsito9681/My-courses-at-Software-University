using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.User;

namespace LogisticsSystem.Data.Models
{
    public class User:IdentityUser
    {
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        public virtual ICollection<Review> Reviews { get; init; }
        = new HashSet<Review>();

        public virtual ICollection<Question> Questions { get; set; }
        = new HashSet<Question>();

        public virtual ICollection<Comment> Comments { get; set; }
        = new HashSet<Comment>();

        public virtual ICollection<Response> Responses { get; set; }
        = new HashSet<Response>();

        public virtual ICollection<Favourite> Favourites { get; set; }
         = new HashSet<Favourite>();

        public virtual ICollection<DeliveryCartItem> DeliveryCartItems { get; set; }
        = new HashSet<DeliveryCartItem>();

        public virtual ICollection<Order> Orders { get; set; }
        = new HashSet<Order>();

        public virtual ICollection<Report> Reports { get; set; }
        = new HashSet<Report>();
    }
}
