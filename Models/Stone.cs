using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Stone
    {
        [Key]
        public int IdStone { get; set; }

        [Required, StringLength(50)]
        public string? NameStone { get; set; }

        [Required, StringLength(50)]
        public string? ColorStone { get; set; }

        [Required, Range(0, 1)]
        public int WeightStone { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
