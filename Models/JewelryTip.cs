using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class JewelryTip
    {
        [Key]
        public int IdJewelryTip {  get; set; }

        [Required, StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern)]
        public string? NameJewelryTip { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
