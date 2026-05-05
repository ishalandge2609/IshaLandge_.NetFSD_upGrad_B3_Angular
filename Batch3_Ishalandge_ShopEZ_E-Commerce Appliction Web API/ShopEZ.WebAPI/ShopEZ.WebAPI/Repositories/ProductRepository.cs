using ShopEZ.WebAPI.Data;
using ShopEZ.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ShopEZ.WebAPI.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor Injection
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET ALL PRODUCTS
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // GET PRODUCT BY ID
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        // ADD PRODUCT
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        // UPDATE PRODUCT
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // DELETE PRODUCT
        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
