using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Material
    {
        [Key]
        public int IdMaterial { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.ShortTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string? NameMaterial { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Range(300, 1000)]
        public int Proba {  get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
