using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Supplier
    {
        [Key]
        public int IdSupplier { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string? NameSupplier { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Phone(ErrorMessage = ValidationMessages.InvalidPhone), StringLength(20, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.PhonePattern, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string? PhoneSupplier { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.MediumTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength), EmailAddress(ErrorMessage = ValidationMessages.InvalidEmail)]
        public string? EmailSupplier { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
