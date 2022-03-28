using LogisticsSystem.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Report;

namespace LogisticsSystem.Data.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public ReportKind ReportKind { get; set; }

        public DateTime PublishedOn { get; set; }

        [Required]
        public string LoadId { get; set; }

        public virtual Load Load { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
