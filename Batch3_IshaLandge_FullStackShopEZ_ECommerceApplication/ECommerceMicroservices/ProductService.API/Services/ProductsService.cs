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

        private readonly ILogger<ProductsService> _logger;

        public ProductsService(
            IProductRepository repo,
            IConfiguration config,
            ILogger<ProductsService> logger)
        {
            _repo = repo;

            _config = config;

            _logger = logger;
        }

        private string GetBaseUrl()
        {
            return _config["BaseUrl"]!;
        }

        private string? BuildImageUrl(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            return $"{GetBaseUrl()}{path}";
        }

        public async Task<(IEnumerable<ProductResponseDTO> Products, int TotalCount)>
            GetAllAsync(int page, int pageSize)
        {
            _logger.LogInformation(
                "Fetching all products. Page: {Page}, PageSize: {PageSize}",
                page,
                pageSize);

            var (products, total) = await _repo
                .GetAllAsync(page, pageSize);

            var dtos = products.Select(p => new ProductResponseDTO
            {
                Id = p.Id,

                Name = p.Name,

                Description = p.Description,

                Price = p.Price,

                Stock = p.Stock,

                Category = p.Category,

                CreatedAt = p.CreatedAt,

                ImageUrl = BuildImageUrl(p.ImageUrl)
            });

            return (dtos, total);
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            _logger.LogInformation(
                "Fetching product details. ProductId: {ProductId}",
                id);

            var p = await _repo.GetByIdAsync(id);

            if (p == null)
            {
                _logger.LogWarning(
                    "Product not found. ProductId: {ProductId}",
                    id);

                return null;
            }

            return new ProductResponseDTO
            {
                Id = p.Id,

                Name = p.Name,

                Description = p.Description,

                Price = p.Price,

                Stock = p.Stock,

                Category = p.Category,

                CreatedAt = p.CreatedAt,

                ImageUrl = BuildImageUrl(p.ImageUrl)
            };
        }

        public async Task<ProductResponseDTO> CreateAsync(CreateProductDTO dto)
        {
            _logger.LogInformation(
                "Creating new product: {ProductName}",
                dto.Name);

            var product = new Product
            {
                Name = dto.Name,

                Description = dto.Description,

                Price = dto.Price,

                Stock = dto.Stock,

                Category = dto.Category,

                ImageUrl = dto.ImageUrl
            };

            var created = await _repo.AddAsync(product);

            _logger.LogInformation(
                "Product created successfully. ProductId: {ProductId}",
                created.Id);

            return new ProductResponseDTO
            {
                Id = created.Id,

                Name = created.Name,

                Description = created.Description,

                Price = created.Price,

                Stock = created.Stock,

                Category = created.Category,

                CreatedAt = created.CreatedAt,

                ImageUrl = BuildImageUrl(created.ImageUrl)
            };
        }

        public async Task UpdateAsync(int id, UpdateProductDTO dto)
        {
            _logger.LogInformation(
                "Updating product. ProductId: {ProductId}",
                id);

            var product = await _repo.GetByIdAsync(id)
                ?? throw new NotFoundException("Product not found");

            product.Name = dto.Name;

            product.Description = dto.Description;

            product.Price = dto.Price;

            product.Stock = dto.Stock;

            product.Category = dto.Category;

            product.ImageUrl = dto.ImageUrl;

            product.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(product);

            _logger.LogInformation(
                "Product updated successfully. ProductId: {ProductId}",
                id);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation(
                "Deleting product. ProductId: {ProductId}",
                id);

            var product = await _repo.GetByIdAsync(id)
                ?? throw new NotFoundException("Product not found");

            await _repo.SoftDeleteAsync(id);

            _logger.LogInformation(
                "Product deleted successfully. ProductId: {ProductId}",
                id);
        }

        // REDUCE STOCK AFTER ORDER
        public async Task ReduceStockAsync(
            int productId,
            int quantity)
        {
            _logger.LogInformation(
                "Reducing stock. ProductId: {ProductId}, Quantity: {Quantity}",
                productId,
                quantity);

            var product = await _repo.GetByIdAsync(productId)
                ?? throw new NotFoundException("Product not found");

            if (product.Stock < quantity)
            {
                _logger.LogWarning(
                    "Insufficient stock for ProductId: {ProductId}",
                    productId);

                throw new ValidationException("Not enough stock");
            }

            product.Stock -= quantity;

            product.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(product);

            _logger.LogInformation(
                "Stock reduced successfully. ProductId: {ProductId}",
                productId);
        }

        // ADMIN RESTOCK
        public async Task RestockProductAsync(
            int productId,
            int quantity)
        {
            _logger.LogInformation(
                "Restocking product. ProductId: {ProductId}, Quantity: {Quantity}",
                productId,
                quantity);

            var product = await _repo.GetByIdAsync(productId)
                ?? throw new NotFoundException("Product not found");

            product.Stock += quantity;

            product.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(product);

            _logger.LogInformation(
                "Product restocked successfully. ProductId: {ProductId}",
                productId);
        }
    }
}