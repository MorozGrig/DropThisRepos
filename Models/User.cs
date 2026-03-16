using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.ShortTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.LoginPattern, ErrorMessage = ValidationMessages.InvalidLogin)]
        public string? Login { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(64, MinimumLength = 6, ErrorMessage = ValidationMessages.InvalidPassword)]
        [RegularExpression(ValidationPatterns.PasswordPattern, ErrorMessage = ValidationMessages.InvalidPassword)]
        public string? Password { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        [Phone(ErrorMessage = ValidationMessages.InvalidPhone)]
        [StringLength(20, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.PhonePattern, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.MediumTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmail)]
        public string? Email { get; set; }

        public int IdRole { get; set; }
        [ForeignKey("IdRole")]
        public Role? Role { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
