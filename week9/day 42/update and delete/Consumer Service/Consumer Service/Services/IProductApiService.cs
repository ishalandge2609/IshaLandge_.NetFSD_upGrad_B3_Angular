using Consumer_Service.Models;

namespace Consumer_Service.Services
{
    public interface IProductApiService
    {
        Task<Product> GetProductId(int id);
        Task<List<Product>> GetProducts();
        Task<Product> AddProduct(Product product);

        Task<Product> UpdateProduct(int id, Product product);
        Task<string> DeleteProduct(int id);
    }
}
