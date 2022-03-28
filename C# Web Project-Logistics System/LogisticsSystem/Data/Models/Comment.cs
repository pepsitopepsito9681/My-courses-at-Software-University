using System;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Comment;

namespace LogisticsSystem.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public bool IsPublic { get; set; }

        public DateTime PublishedOn { get; set; }

        public int ReviewId { get; set; }

        public Review Review { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
