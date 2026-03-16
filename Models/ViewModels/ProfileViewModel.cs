using DropThisSite.Models;
using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.ShortTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.LoginPattern, ErrorMessage = ValidationMessages.InvalidLogin)]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.MediumTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmail)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(20, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.PhonePattern, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string Phone { get; set; } = string.Empty;

        public List<Order> Orders { get; set; } = new();
    }
}
