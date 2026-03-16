using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class StatusOrder
    {
        [Key]
        public int IdStatusOrder { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string? NameStatusOrder { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
