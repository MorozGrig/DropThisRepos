using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        [StringLength(ValidationPatterns.MediumTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [RegularExpression(ValidationPatterns.PhonePattern)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(ValidationPatterns.MediumTextMaxLength)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(ValidationPatterns.LongTextMaxLength)]
        [RegularExpression(ValidationPatterns.AddressPattern)]
        public string Address { get; set; } = string.Empty;
    }
}
