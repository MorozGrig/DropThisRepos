using DropThisSite.Models;
using Microsoft.EntityFrameworkCore;

namespace DropThisSite.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Stone> Stones { get; set; }

        public DbSet<JewelryTip> JewelryTips { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Jewelry> Jewelries { get; set; }

        public DbSet<StatusOrder> StatusOrders { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
