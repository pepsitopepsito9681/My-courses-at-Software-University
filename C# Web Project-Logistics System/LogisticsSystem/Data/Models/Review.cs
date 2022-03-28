using LogisticsSystem.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Review;

namespace LogisticsSystem.Data.Models
{
    public class Review
    {
        public int Id { get; init; }

        [Required]
        public ReviewKind Rating { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public bool IsPublic { get; set; }

        public DateTime PublishedOn { get; set; }

        [Required]
        public string LoadId { get; set; }

        public virtual Load Load { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
         = new HashSet<Comment>();
    }
}
