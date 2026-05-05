using ShopEZ.WebAPI.DTOs;
using ShopEZ.WebAPI.Models;

namespace ShopEZ.WebAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAllAsync();

        Task<ProductResponseDTO> GetByIdAsync(int id);

        Task<ProductResponseDTO> CreateAsync(ProductCreateDTO dto);

        Task<bool> UpdateAsync(int id, ProductCreateDTO dto);

        Task<bool> DeleteAsync(int id);

        Task<object> GetProductsAsync(ProductQueryDTO query);
    }
}
