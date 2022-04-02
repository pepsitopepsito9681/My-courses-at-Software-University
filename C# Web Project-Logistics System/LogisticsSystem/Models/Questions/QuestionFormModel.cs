using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Question;

namespace LogisticsSystem.Models.Questions
{
    public class QuestionFormModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
        [Display(Name = "Question:")]
        public string Content { get; set; }
    }
}
