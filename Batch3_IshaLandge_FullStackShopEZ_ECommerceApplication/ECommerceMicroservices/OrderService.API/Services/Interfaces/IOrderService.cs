using OrderService.API.DTOs;

namespace OrderService.API.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(
            int userId,
            CreateOrderDTO dto,
            string token);

        Task<OrderResponseDTO?> GetByIdAsync(int orderId);

        Task DeleteAsync(
            int orderId,
            int userId,
            bool isAdmin);

        Task<IEnumerable<OrderResponseDTO>>
            GetOrdersByUserIdAsync(int userId);

        Task<IEnumerable<OrderResponseDTO>>
            GetAllOrdersAsync();

        Task CancelOrderAsync(
            int orderId,
            int userId,
            bool isAdmin,
            string token);
    }
}