using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required]
        [StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.LoginPattern)]
        public string? Login { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 6)]
        [RegularExpression(ValidationPatterns.PasswordPattern)]
        public string? Password { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        [RegularExpression(ValidationPatterns.PhonePattern)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(ValidationPatterns.MediumTextMaxLength)]
        [EmailAddress]
        public string? Email { get; set; }

        public int IdRole { get; set; }
        [ForeignKey("IdRole")]
        public Role? Role { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
