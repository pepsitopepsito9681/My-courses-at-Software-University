using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Services.Loads.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LogisticsSystem.Data.DataConstants.Load;

namespace LogisticsSystem.Models.Loads
{
    public class LoadFormModel
    {
        [Required]
        [StringLength(TittleMaxLength, MinimumLength = TittleMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string Title { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "The field {0} must be between {2} and {1} characters long")]
        public string Description { get; init; }

        [Range(PriceMinLength, PriceMaxLength, ErrorMessage = "The field {0} must be between {1:f2} and {2}")]
        public decimal Price { get; init; }

        [Range(QuantityMinLength, byte.MaxValue)]
        public byte Quantity { get; init; }


        [Required]
        [Display(Name = "First Image Url")]
        [Url]
        public string FirstImageUrl { get; set; }

        [Display(Name = "Second Image Url(Optional)")]
        [Url]
        public string SecondImageUrl { get; set; }

        [Display(Name = "Third Image Url(Optional)")]
        [Url]
        public string ThirdImageUr { get; set; }

        [Required(ErrorMessage = "Please select a Kind from the list.")]
        [Display(Name = "Kind")]
        public int KindId { get; set; }

        [Required(ErrorMessage = "Please select a subKind from the list.")]
        [Display(Name = "SubKind")]
        public int SubKindId { get; set; }

        [Required(ErrorMessage = "Please select a condition from the list.")]
        [EnumDataType(typeof(LoadCondition))]
        public LoadCondition? Condition { get; set; }

        [Required(ErrorMessage = "Please select a person type from the list.")]
        [EnumDataType(typeof(PersonType))]
        public PersonType? Delivery { get; set; }

        public bool AgreeOnTermsOfPolitics { get; set; }

        public IEnumerable<LoadKindServiceModel> Kinds { get; set; }

        public IEnumerable<LoadSubKindServiceModel> SubKinds { get; set; }
    }
}
