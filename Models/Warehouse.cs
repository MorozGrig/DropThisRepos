using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Warehouse
    {
        [Key]
        public int IdWarehouse { get; set; }

        [Required]
        [StringLength(100)]
        public string NameWarehouse { get; set; } = string.Empty;

        public ICollection<WarehouseItem>? WarehouseItems { get; set; }
    }
}