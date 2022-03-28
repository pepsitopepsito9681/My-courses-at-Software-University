using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static LogisticsSystem.Data.DataConstants.Trader;

namespace LogisticsSystem.Data.Models
{
    public class Trader
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(TelephoneNumberMaxLength)]
        public string TelephoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        [NotMapped]
        public virtual User User { get; set; }

        public virtual IEnumerable<Load> Loads { get; set; }
    }
}
