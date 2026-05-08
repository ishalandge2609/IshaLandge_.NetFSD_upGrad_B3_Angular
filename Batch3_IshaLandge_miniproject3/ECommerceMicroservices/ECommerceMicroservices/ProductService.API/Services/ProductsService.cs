using ProductService.API.Exceptions;
using ProductService.API.DTOs;
using ProductService.API.Models;
using ProductService.API.Repositories;

namespace ProductService.API.Services
{
    public class ProductsService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IConfiguration _config;

        public ProductsService(IProductRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

       
        private string GetBaseUrl()
        {
            return _config["BaseUrl"]!;
        }

        
        private string? BuildImageUrl(string? path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            return $"{GetBaseUrl()}{path}";
        }

        public async Task<(IEnumerable<ProductResponseDTO> Products, int TotalCount)> GetAllAsync(int page, int pageSize)
        {
            var (products, total) = await _repo.GetAllAsync(page, pageSize);

            var dtos = products.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CreatedAt = p.CreatedAt,
                ImageUrl = BuildImageUrl(p.ImageUrl)
            });

            return (dtos, total);
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);

            if (p == null) return null;

            return new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CreatedAt = p.CreatedAt,
                ImageUrl = BuildImageUrl(p.ImageUrl)
            };
        }

        public async Task<ProductResponseDTO> CreateAsync(CreateProductDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl // relative path only
            };

            var created = await _repo.AddAsync(product);

            return new ProductResponseDTO
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price,
                Stock = created.Stock,
                CreatedAt = created.CreatedAt,
                ImageUrl = BuildImageUrl(created.ImageUrl)
            };
        }

        public async Task UpdateAsync(int id, UpdateProductDTO dto)
        {
            var product = await _repo.GetByIdAsync(id)
                ?? throw new NotFoundException("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.ImageUrl = dto.ImageUrl;
            product.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id)
                ?? throw new NotFoundException("Product not found");

            await _repo.SoftDeleteAsync(id);
        }
        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _repo.GetByIdAsync(productId)
                ?? throw new NotFoundException("Product not found");

            if (product.Stock < quantity)
                throw new ValidationException("Not enough stock");

            product.Stock -= quantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(product);
        }
    }
}