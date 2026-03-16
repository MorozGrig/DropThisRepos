using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class OrderItem
    {
        [Key]
        public int IdOrderItem { get; set; }

        [Range(1, int.MaxValue)]
        public int IdOrder { get; set; }
        [ForeignKey("IdOrder")]
        public Order? Order { get; set; }

        [Range(1, int.MaxValue)]
        public int IdJewelry { get; set; }
        [ForeignKey("IdJewelry")]
        public Jewelry? Jewelry { get; set; }

        [Required, Range(1, ValidationPatterns.QuantityMaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0, ValidationPatterns.PriceMaxValue * ValidationPatterns.QuantityMaxValue)]
        public int UnitPrice { get; set; }

        [Required, Range(0, ValidationPatterns.PriceMaxValue * ValidationPatterns.QuantityMaxValue)]
        public int TotalPrice { get; set; }
    }
}
