using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required]
        [StringLength(50)]
        public string? Login { get; set; }

        [Required]
        [StringLength(20)]
        public string? Password { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        public int IdRole { get; set; }
        [ForeignKey("IdRole")]
        public Role? Role { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
