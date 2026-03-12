using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class StatusOrder
    {
        [Key]
        public int IdStatusOrder { get; set; }

        [Required, StringLength(50)]
        public string? NameStatusOrder { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
