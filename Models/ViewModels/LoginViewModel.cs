using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.ShortTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.LoginPattern, ErrorMessage = ValidationMessages.InvalidLogin)]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(64, MinimumLength = 6, ErrorMessage = ValidationMessages.InvalidPassword)]
        [RegularExpression(ValidationPatterns.PasswordPattern, ErrorMessage = ValidationMessages.InvalidPassword)]
        public string Password { get; set; } = string.Empty;
    }
}
