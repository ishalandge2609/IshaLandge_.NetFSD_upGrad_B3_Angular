using Microsoft.EntityFrameworkCore;
using OrderService.API.Data;
using OrderService.API.Models;

namespace OrderService.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        public OrderRepository(OrderDbContext context) => _context = context;

        public async Task<Order?> GetByIdAsync(int id) =>
            await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Order> AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId && !o.IsDeleted)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}