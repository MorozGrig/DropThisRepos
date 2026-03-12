using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Role
    {
        [Key]
        public int IdRole { get; set; }

        [Required]
        [StringLength(50)]
        public string? NameRole { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
