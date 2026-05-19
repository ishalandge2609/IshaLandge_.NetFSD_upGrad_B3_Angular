using ProductService.API.Models;

namespace ProductService.API.Repositories
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(int page, int pageSize);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task SoftDeleteAsync(int id);
    }
}