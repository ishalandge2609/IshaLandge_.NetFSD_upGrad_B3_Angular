using ShopEZ.WebAPI.DTOs;
using ShopEZ.WebAPI.Models;
using ShopEZ.WebAPI.Repositories;

namespace ShopEZ.WebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        //  GET ALL PRODUCTS
        public async Task<IEnumerable<ProductResponseDTO>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();

            return products.Select(p => new ProductResponseDTO
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Stock = p.Stock
            });
        }

        // GET PRODUCT BY ID
        public async Task<ProductResponseDTO> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductResponseDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock
            };
        }

        // CREATE PRODUCT
        public async Task<ProductResponseDTO> CreateAsync(ProductCreateDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Stock = dto.Stock
            };

            await _repository.AddAsync(product);

            return new ProductResponseDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock
            };
        }

        // UPDATE PRODUCT
        public async Task<bool> UpdateAsync(int id, ProductCreateDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return false;

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.ImageUrl = dto.ImageUrl;
            existing.Stock = dto.Stock;

            await _repository.UpdateAsync(existing);

            return true;
        }

        // DELETE PRODUCT
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return false;

            await _repository.DeleteAsync(product);

            return true;
        }

        // PAGINATION + SEARCH + FILTER
        public async Task<object> GetProductsAsync(ProductQueryDTO query)
        {
            var products = (await _repository.GetAllAsync()).ToList();

            // SEARCH (by name)
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.ToLower();

                products = products
                    .Where(p => !string.IsNullOrEmpty(p.Name) &&
                                p.Name.ToLower().Contains(search))
                    .ToList();
            }

            // FILTER BY PRICE
            if (query.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= query.MinPrice.Value).ToList();
            }

            if (query.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= query.MaxPrice.Value).ToList();
            }

            // PAGINATION
            var totalRecords = products.Count;

            var pagedData = products
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            //  MAP
            var result = pagedData.Select(p => new ProductResponseDTO
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Stock = p.Stock
            });

            return new
            {
                TotalRecords = totalRecords,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                Data = result
            };
        }
    }
}