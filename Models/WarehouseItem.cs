using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class WarehouseItem
    {
        [Key]
        public int IdWarehouseItem { get; set; }

        public int IdWarehouse { get; set; }
        [ForeignKey("IdWarehouse")]
        public Warehouse? Warehouse { get; set; }

        public int IdJewelry { get; set; }
        [ForeignKey("IdJewelry")]
        public Jewelry? Jewelry { get; set; }

        [Required]
        [Range(0, 10000)]
        public int Quantity { get; set; }
    }
}