using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class Stone
    {
        [Key]
        public int IdStone { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string? NameStone { get; set; }

        public int IdColorStone { get; set; }
        [ForeignKey("IdColorStone")]
        public ColorStoune? ColorStoune { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        [Range(typeof(float), "0", "1", ErrorMessage = ValidationMessages.InvalidWeight)]
        public float WeightStone { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
