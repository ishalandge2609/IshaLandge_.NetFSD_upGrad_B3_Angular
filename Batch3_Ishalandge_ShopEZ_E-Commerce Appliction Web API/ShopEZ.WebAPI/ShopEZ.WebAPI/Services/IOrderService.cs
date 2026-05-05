using ShopEZ.WebAPI.DTOs;
using ShopEZ.WebAPI.Models;

namespace ShopEZ.WebAPI.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(OrderCreateDTO dto);

        Task<IEnumerable<OrderResponseDTO>> GetAllAsync();

        Task<OrderResponseDTO> GetByIdAsync(int id);
    }
}
