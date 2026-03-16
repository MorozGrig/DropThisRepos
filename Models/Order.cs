using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class Order
    {
        [Key]
        [Range(1, int.MaxValue)]
        public int IdOrder { get; set; }

        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public User? User { get; set; }

        public int? IdJewelry { get; set; }
        [ForeignKey("IdJewelry")]
        public Jewelry? Jewelry { get; set; }

        [Required, StringLength(ValidationPatterns.MediumTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern)]
        public string? CustomerName { get; set; }

        [Required, StringLength(20)]
        [Phone]
        [RegularExpression(ValidationPatterns.PhonePattern)]
        public string? CustomerPhone { get; set; }

        [Required, StringLength(ValidationPatterns.MediumTextMaxLength)]
        [EmailAddress]
        public string? CustomerEmail { get; set; }

        [Required, StringLength(ValidationPatterns.LongTextMaxLength)]
        [RegularExpression(ValidationPatterns.AddressPattern)]
        public string? DeliveryAddress { get; set; }

        public int IdStatusOrder { get; set; }
        [ForeignKey("IdStatusOrder")]
        public StatusOrder? StatusOrder { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required, Range(1, ValidationPatterns.QuantityMaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0, ValidationPatterns.PriceMaxValue * ValidationPatterns.QuantityMaxValue)]
        public int TotalPrice { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
