using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Supplier
    {
        [Key]
        public int IdSupplier { get; set; }

        [Required, StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern)]
        public string? NameSupplier { get; set; }

        [Required, Phone, StringLength(20)]
        [RegularExpression(ValidationPatterns.PhonePattern)]
        public string? PhoneSupplier { get; set; }

        [Required, StringLength(ValidationPatterns.MediumTextMaxLength), EmailAddress]
        public string? EmailSupplier { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
