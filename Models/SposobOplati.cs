using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class SposobOplati
    {
        [Key]
        public int IdSposobOplati { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string? NameSposobOplati { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
