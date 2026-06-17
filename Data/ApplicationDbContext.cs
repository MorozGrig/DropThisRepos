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

        public DbSet<ColorStoune> ColorsStounes { get; set; }

        public DbSet<Stone> Stones { get; set; }

        public DbSet<JewelryTip> JewelryTips { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Jewelry> Jewelries { get; set; }

        public DbSet<SposobOplati> SposobiOplati { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<WarehouseItem> WarehouseItems { get; set; }

        public DbSet<StatusOrder> StatusOrders { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.IdOrder)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Deliveries)
                .WithOne(d => d.Order)
                .HasForeignKey<Delivery>(d => d.IdOrder)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Jewelry)
                .WithMany(j => j.OrderItems)
                .HasForeignKey(oi => oi.IdJewelry)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WarehouseItem>()
                .HasOne(wi => wi.Warehouse)
                .WithMany(w => w.WarehouseItems)
                .HasForeignKey(wi => wi.IdWarehouse)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WarehouseItem>()
                .HasOne(wi => wi.Jewelry)
                .WithMany(j => j.WarehouseItems)
                .HasForeignKey(wi => wi.IdJewelry)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
