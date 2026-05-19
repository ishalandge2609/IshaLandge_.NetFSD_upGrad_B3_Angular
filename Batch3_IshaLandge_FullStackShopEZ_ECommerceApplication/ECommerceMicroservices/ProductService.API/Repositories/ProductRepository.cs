using Microsoft.EntityFrameworkCore;
using ProductService.API.Data;
using ProductService.API.Models;

namespace ProductService.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        public ProductRepository(ProductDbContext context) => _context = context;

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(int page, int pageSize)
        {
            var query = _context.Products.AsNoTracking();
            int total = await query.CountAsync();
            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (products, total);
        }

        public async Task<Product?> GetByIdAsync(int id) =>
            await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                product.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}