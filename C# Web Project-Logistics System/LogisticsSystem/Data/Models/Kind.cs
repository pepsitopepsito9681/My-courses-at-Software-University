using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Kind;

namespace LogisticsSystem.Data.Models
{
    public class Kind
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<SubKind> SubKinds { get; set; }

        public virtual ICollection<Load> Loads { get; set; }
    }
}
