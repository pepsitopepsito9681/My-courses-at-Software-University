using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Question;

namespace LogisticsSystem.Data.Models
{
    public class Question
    {
        public int Id { get; init; }

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

        public virtual ICollection<Response> Responses { get; set; }
        = new HashSet<Response>();
    }
}
