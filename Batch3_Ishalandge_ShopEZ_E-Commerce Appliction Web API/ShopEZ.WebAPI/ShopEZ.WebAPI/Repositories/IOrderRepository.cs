using ShopEZ.WebAPI.Models;

namespace ShopEZ.WebAPI.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);

        Task<IEnumerable<Order>> GetAllAsync();

        Task<Order> GetByIdAsync(int id);
    }
}
