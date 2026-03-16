using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Role
    {
        [Key]
        public int IdRole { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        [StringLength(ValidationPatterns.ShortTextMaxLength, ErrorMessage = ValidationMessages.InvalidLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern, ErrorMessage = ValidationMessages.InvalidText)]
        public string? NameRole { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
