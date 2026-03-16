using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Stone
    {
        [Key]
        public int IdStone { get; set; }

        [Required, StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern)]
        public string? NameStone { get; set; }

        [Required, StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern)]
        public string? ColorStone { get; set; }

        [Required, Range(0, 1)]
        public float WeightStone { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
