using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class JewelryTip
    {
        [Key]
        public int IdJewelryTip {  get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string? NameJewelryTip { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
