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

        [Required(ErrorMessage = ValidationMessages.Required), Phone, StringLength(20)]
        [RegularExpression(ValidationPatterns.PhonePattern, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string? PhoneSupplier { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.MediumTextMaxLength), EmailAddress]
        public string? EmailSupplier { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
