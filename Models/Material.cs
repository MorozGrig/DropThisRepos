using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Material
    {
        [Key]
        public int IdMaterial { get; set; }

        [Required]
        [StringLength(50)]
        public string? NameMaterial { get; set; }

        [Required, Range(300, 1000)]
        public int Proba {  get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
