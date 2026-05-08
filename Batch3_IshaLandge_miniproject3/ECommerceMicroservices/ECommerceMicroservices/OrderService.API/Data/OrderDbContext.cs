using Microsoft.EntityFrameworkCore;
using OrderService.API.Models;

namespace OrderService.API.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Order>().HasMany(o => o.OrderItems).WithOne(i => i.Order).HasForeignKey(i => i.OrderId);
            modelBuilder.Entity<OrderItem>().Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
        }
    }
}