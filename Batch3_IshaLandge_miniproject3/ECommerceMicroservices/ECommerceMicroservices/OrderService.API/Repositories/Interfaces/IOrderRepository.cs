using OrderService.API.Models;

namespace OrderService.API.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task<Order> AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task SoftDeleteAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
    }
}