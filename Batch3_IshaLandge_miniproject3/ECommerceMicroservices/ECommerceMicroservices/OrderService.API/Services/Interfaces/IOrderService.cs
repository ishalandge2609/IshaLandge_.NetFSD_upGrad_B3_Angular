using OrderService.API.DTOs;

namespace OrderService.API.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(int userId, CreateOrderDTO dto, string token);
        Task<OrderResponseDTO?> GetByIdAsync(int id);
        Task DeleteAsync(int orderId, int userId, bool isAdmin);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId);
    }
}