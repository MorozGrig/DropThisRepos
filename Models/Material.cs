using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Material
    {
        [Key]
        public int IdMaterial { get; set; }

        [Required]
        [StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern)]
        public string? NameMaterial { get; set; }

        [Required, Range(300, 1000)]
        public int Proba {  get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
