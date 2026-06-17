using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class Order
    {
        [Key]
        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.InvalidSelection)]
        public int IdOrder { get; set; }

        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public User? User { get; set; }

        public int? IdJewelry { get; set; }
        [ForeignKey("IdJewelry")]
        public Jewelry? Jewelry { get; set; }

        public int IdStatusOrder { get; set; }
        [ForeignKey("IdStatusOrder")]
        public StatusOrder? StatusOrder { get; set; }

        public int IdSposobOplati { get; set; }
        [ForeignKey("IdSposobOplati")]
        public SposobOplati? SposobOplati { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Range(1, ValidationPatterns.QuantityMaxValue, ErrorMessage = ValidationMessages.InvalidQuantity)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Range(1, ValidationPatterns.PriceMaxValue * ValidationPatterns.QuantityMaxValue, ErrorMessage = ValidationMessages.InvalidPrice)]
        public int TotalPrice { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Delivery? Deliveries { get; set; }
    }
}
