using System.ComponentModel.DataAnnotations;

namespace DropThisSite.Models
{
    public class Supplier
    {
        [Key]
        public int IdSupplier { get; set; }

        [Required, StringLength(50)]
        public string? NameSupplier { get; set; }

        [Required, Phone]
        public string? PhoneSupplier { get; set; }

        [Required, EmailAddress]
        public string? EmailSupplier { get; set; }

        public ICollection<Jewelry>? Jewelries { get; set; }
    }
}
