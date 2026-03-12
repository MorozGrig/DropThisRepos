using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class JewelryTip
    {
        [Key]
        public int IdJewelryTip {  get; set; }

        [Required, StringLength(50)]
        public string? NameJewelryTip { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
