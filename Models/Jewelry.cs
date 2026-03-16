using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class Jewelry
    {
        [Key]
        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.InvalidSelection)]
        public int IdJewelry { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string? NameJewelry { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.InvalidSelection)]
        public int IdJewelryTip { get; set; }
        [ForeignKey("IdJewelryTip")]
        public JewelryTip? JewelryTip { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.InvalidSelection)]
        public int IdMaterial {  get; set; }
        [ForeignKey("IdMaterial")]
        public Material? Material { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.InvalidSelection)]
        public int IdStone { get; set; }
        [ForeignKey("IdStone")]
        public Stone? Stone { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.InvalidSelection)]
        public int IdSupplier { get; set; }
        [ForeignKey("IdSupplier")]
        public Supplier? Supplier { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Range(1, ValidationPatterns.PriceMaxValue, ErrorMessage = ValidationMessages.InvalidPrice)]
        public int PriceJewelry { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
