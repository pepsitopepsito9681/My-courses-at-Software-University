using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Kind;

namespace LogisticsSystem.Data.Models
{
    public class SubKind
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public int KindId { get; set; }

        public virtual Kind Kind { get; set; }

        public virtual ICollection<Load> Loads { get; set; }
    }
}
