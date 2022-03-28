using System.ComponentModel.DataAnnotations;

namespace LogisticsSystem.Data.Models
{
    public class LoadImage
    {
        public int Id { get; set; }

        [Required]
        public string LoadId { get; set; }

        public virtual Load Load { get; set; }

        public string ImageUrl { get; set; }
    }
}
