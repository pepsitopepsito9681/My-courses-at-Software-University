using LogisticsSystem.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Review;

namespace LogisticsSystem.Models.Reviews
{
    public class ReviewFormModel
    {
        [Required(ErrorMessage = "Please select a rating from the list.")]
        [EnumDataType(typeof(ReviewKind))]
        public ReviewKind? Rating { get; set; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
        [Display(Name = "Review:")]
        public string Content { get; set; }

        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
        public string Title { get; set; }
    }
}
