using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class OrderItem
    {
        [Key]
        public int IdOrderItem { get; set; }

        public int IdOrder { get; set; }
        [ForeignKey("IdOrder")]
        public Order? Order { get; set; }

        public int IdJewelry { get; set; }
        [ForeignKey("IdJewelry")]
        public Jewelry? Jewelry { get; set; }

        [Required, Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        public int UnitPrice { get; set; }

        [Required]
        public int TotalPrice { get; set; }
    }
}
