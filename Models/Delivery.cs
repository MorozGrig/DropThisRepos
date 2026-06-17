using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class Delivery
    {
        [Key]
        public int IdDelivery {  get; set; }

        public int IdOrder { get; set; }
        [ForeignKey("IdOrder")]
        public Order? Order { get; set; }

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
    }
}
