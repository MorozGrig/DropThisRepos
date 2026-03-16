using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.LoginPattern)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [StringLength(ValidationPatterns.MediumTextMaxLength)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(64, MinimumLength = 6)]
        [RegularExpression(ValidationPatterns.PasswordPattern)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [RegularExpression(ValidationPatterns.PhonePattern)]
        public string Phone { get; set; } = string.Empty;
    }
}
