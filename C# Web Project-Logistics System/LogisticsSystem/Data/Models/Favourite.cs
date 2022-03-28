using System.ComponentModel.DataAnnotations;

namespace LogisticsSystem.Data.Models
{
    public class Favourite
    {
        public int Id { get; set; }

        [Required]
        public string LoadId { get; set; }

        public virtual Load Load { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
