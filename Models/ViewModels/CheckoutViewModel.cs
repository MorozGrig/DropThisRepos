using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.MediumTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(20, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.PhonePattern, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.MediumTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmail)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.LongTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.AddressPattern, ErrorMessage = ValidationMessages.InvalidAddress)]
        public string Address { get; set; } = string.Empty;
    }
}
