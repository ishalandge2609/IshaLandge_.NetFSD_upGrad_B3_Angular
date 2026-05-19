using OrderService.API.Models;

namespace OrderService.API.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int orderId);

        Task<Order> AddAsync(Order order);

        Task UpdateAsync(Order order);

        Task SoftDeleteAsync(int orderId);

        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);

        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task CancelOrderAsync(Order order);
    }
}