using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Trader;

namespace LogisticsSystem.Models.Traders
{
    public class BecomeTraderFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        [RegularExpression(@"^08[789]\d{7}$", ErrorMessage = "{0} must be in format 08[7-9].......")]
        public string TelephoneNumber { get; set; }
    }
}
