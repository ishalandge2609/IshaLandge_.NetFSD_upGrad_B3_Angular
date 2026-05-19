using ProductService.API.DTOs;

namespace ProductService.API.Services
{
    public interface IProductService
    {
        Task<(IEnumerable<ProductResponseDTO> Products, int TotalCount)>
            GetAllAsync(int page, int pageSize);

        Task<ProductResponseDTO?> GetByIdAsync(int id);

        Task<ProductResponseDTO> CreateAsync(CreateProductDTO dto);

        Task UpdateAsync(int id, UpdateProductDTO dto);

        Task DeleteAsync(int id);

        // REDUCE STOCK AFTER ORDER
        Task ReduceStockAsync(int productId, int quantity);

        // ADMIN RESTOCK
        Task RestockProductAsync(int productId, int quantity);
    }
}