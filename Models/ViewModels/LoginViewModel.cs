using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.LoginPattern)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [StringLength(64, MinimumLength = 6)]
        [RegularExpression(ValidationPatterns.PasswordPattern)]
        public string Password { get; set; } = string.Empty;
    }
}
