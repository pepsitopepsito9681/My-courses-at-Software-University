using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Order;

namespace LogisticsSystem.Services.Orders.Models
{
    public class OrderFormServiceModel
    {
        [Display(Name = "Full Name")]
        [Required]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; }

        [Required]
        [StringLength(AddressMаxLength, MinimumLength = AddressMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string Address { get; init; }

        [Required]
        [StringLength(StateMаxLength, MinimumLength = StateMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string State { get; init; }

        [Required]
        [StringLength(CityMаxLength, MinimumLength = CityMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string City { get; init; }

        [Display(Name = "Post Code")]
        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "{0} must be 4 digits")]
        public string PostCode { get; init; }

        [Display(Name = "Phone Number")]
        [Required]
        [RegularExpression(@"^08[789]\d{7}$", ErrorMessage = "{0} must be in format 08[7-9].......")]
        public string TelephoneNumber { get; set; }
    }
}
