using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Response;

namespace LogisticsSystem.Models.Responses
{
    public class ResponseFormModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength =ContentMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
    [Display(Name = "Response: ")]
    public string Content { get; set; }
    }
}
