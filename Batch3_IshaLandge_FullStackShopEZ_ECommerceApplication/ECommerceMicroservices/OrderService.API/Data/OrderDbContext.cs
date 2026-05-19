using Microsoft.EntityFrameworkCore;
using OrderService.API.Models;

namespace OrderService.API.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(
            DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        // Orders table
        public DbSet<Order> Orders => Set<Order>();

        // OrderItems table
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary key for Orders table
            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderId);

            // Primary key for OrderItems table
            modelBuilder.Entity<OrderItem>()
                .HasKey(i => i.OrderItemId);

            // Excludes soft deleted orders from queries
            modelBuilder.Entity<Order>()
                .HasQueryFilter(o => !o.IsDeleted);

            // One-to-many relationship between Order and OrderItems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);

            // Decimal precision for UnitPrice
            modelBuilder.Entity<OrderItem>()
                .Property(i => i.UnitPrice)
                .HasColumnType("decimal(18,2)");

            // Decimal precision for TotalAmount
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");
        }
    }
}