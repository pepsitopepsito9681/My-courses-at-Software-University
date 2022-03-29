using LogisticsSystem.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Report;

namespace LogisticsSystem.Models.Reports
{
    public class ReportFormModel
    {
        [Required(ErrorMessage = "Please select a rating from the list.")]
        [EnumDataType(typeof(ReportKind))]
        public ReportKind? ReportKind { get; set; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
        [Display(Name = "Report:")]
        public string Content { get; set; }
    }
}
