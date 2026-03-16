using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class OrderItem
    {
        [Key]
        public int IdOrderItem { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.InvalidSelection)]
        public int IdOrder { get; set; }
        [ForeignKey("IdOrder")]
        public Order? Order { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.InvalidSelection)]
        public int IdJewelry { get; set; }
        [ForeignKey("IdJewelry")]
        public Jewelry? Jewelry { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Range(1, ValidationPatterns.QuantityMaxValue, ErrorMessage = ValidationMessages.InvalidQuantity)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Range(1, ValidationPatterns.PriceMaxValue * ValidationPatterns.QuantityMaxValue, ErrorMessage = ValidationMessages.InvalidPrice)]
        public int UnitPrice { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Range(1, ValidationPatterns.PriceMaxValue * ValidationPatterns.QuantityMaxValue, ErrorMessage = ValidationMessages.InvalidPrice)]
        public int TotalPrice { get; set; }
    }
}
