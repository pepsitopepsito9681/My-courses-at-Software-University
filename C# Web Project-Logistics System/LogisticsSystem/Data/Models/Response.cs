﻿using System;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Response;

namespace LogisticsSystem.Data.Models
{
    public class Response
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public bool IsPublic { get; set; }

        public DateTime PublishedOn { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
