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

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.MediumTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(20)]
        [Phone(ErrorMessage = ValidationMessages.InvalidPhone)]
        [RegularExpression(ValidationPatterns.PhonePattern, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string? CustomerPhone { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.MediumTextMaxLength)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmail)]
        public string? CustomerEmail { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), StringLength(ValidationPatterns.LongTextMaxLength)]
        [RegularExpression(ValidationPatterns.AddressPattern, ErrorMessage = ValidationMessages.InvalidAddress)]
        public string? DeliveryAddress { get; set; }

        public int IdStatusOrder { get; set; }
        [ForeignKey("IdStatusOrder")]
        public StatusOrder? StatusOrder { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Range(1, ValidationPatterns.QuantityMaxValue, ErrorMessage = ValidationMessages.InvalidQuantity)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required), Range(1, ValidationPatterns.PriceMaxValue * ValidationPatterns.QuantityMaxValue, ErrorMessage = ValidationMessages.InvalidPrice)]
        public int TotalPrice { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
