using NuGet.Protocol;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class Jewelry
    {
        [Key]
        public int IdJewelry { get; set; }

        [Required, StringLength(50)]
        public string? NameJewelry { get; set; }

        public int IdJewelryTip { get; set; }
        [ForeignKey("IdJewelryTip")]
        public JewelryTip? JewelryTip { get; set; }

        public int IdMaterial {  get; set; }
        [ForeignKey("IdMaterial")]
        public Material? Material { get; set; }

        public int IdStone { get; set; }
        [ForeignKey("IdStone")]
        public Stone? Stone { get; set; }

        public int IdSupplier { get; set; }
        [ForeignKey("IdSupplier")]
        public Supplier? Supplier { get; set; }

        [Required, Range(10000, 100000)]
        public int PriceJewelry { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
