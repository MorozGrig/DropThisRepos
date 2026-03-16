using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropThisSite.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }

        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public User? User { get; set; }

        public int? IdJewelry { get; set; }
        [ForeignKey("IdJewelry")]
        public Jewelry? Jewelry { get; set; }

        [StringLength(100)]
        public string? CustomerName { get; set; }

        [StringLength(30)]
        public string? CustomerPhone { get; set; }

        [StringLength(100)]
        public string? CustomerEmail { get; set; }

        [StringLength(250)]
        public string? DeliveryAddress { get; set; }

        public int IdStatusOrder { get; set; }
        [ForeignKey("IdStatusOrder")]
        public StatusOrder? StatusOrder { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required, Range(0, 100)]
        public int Quantity { get; set; }

        [Required]
        public int TotalPrice { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
