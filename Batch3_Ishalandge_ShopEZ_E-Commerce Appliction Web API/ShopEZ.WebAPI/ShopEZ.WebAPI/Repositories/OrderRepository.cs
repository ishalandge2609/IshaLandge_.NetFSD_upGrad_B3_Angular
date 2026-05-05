using Microsoft.EntityFrameworkCore;
using ShopEZ.WebAPI.Data;
using ShopEZ.WebAPI.Models;

namespace ShopEZ.WebAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE ORDER
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        // GET ALL ORDERS
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
               .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.Product) // IMPORTANT
               .ToListAsync();
        }

        // GET ORDER BY ID
        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
             .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
    .FirstOrDefaultAsync(o => o.OrderId == id);
        }
    }
}
