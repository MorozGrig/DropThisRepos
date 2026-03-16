using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Role
    {
        [Key]
        public int IdRole { get; set; }

        [Required]
        [StringLength(ValidationPatterns.ShortTextMaxLength)]
        [RegularExpression(ValidationPatterns.SafeTextPattern)]
        public string? NameRole { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
