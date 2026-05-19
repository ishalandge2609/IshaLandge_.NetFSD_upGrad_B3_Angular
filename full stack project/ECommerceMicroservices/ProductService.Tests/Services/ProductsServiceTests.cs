using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ProductService.API.DTOs;
using ProductService.API.Exceptions;
using ProductService.API.Models;
using ProductService.API.Repositories;
using ProductService.API.Services;
using Xunit;

namespace ProductService.Tests.Services
{
    public class ProductsServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo;

        private readonly Mock<ILogger<ProductsService>> _mockLogger;

        private readonly IConfiguration _configuration;

        private readonly ProductsService _service;

        public ProductsServiceTests()
        {
            _mockRepo =
                new Mock<IProductRepository>();

            _mockLogger =
                new Mock<ILogger<ProductsService>>();

            var configData =
                new Dictionary<string, string>
                {
                    {
                        "BaseUrl",
                        "https://localhost:7210"
                    }
                };

            _configuration =
                new ConfigurationBuilder()
                .AddInMemoryCollection(configData!)
                .Build();

            _service = new ProductsService(
                _mockRepo.Object,
                _configuration,
                _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProducts()
        {
            // Arrange

            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Chair",
                    Description = "Wooden chair",
                    Price = 5000,
                    Stock = 10,
                    Category = "Furniture"
                }
            };

            _mockRepo
                .Setup(r => r.GetAllAsync(1, 10))
                .ReturnsAsync((products, 1));

            // Act

            var result =
                await _service.GetAllAsync(1, 10);

            // Assert

            Assert.Single(result.Products);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange

            var products = new List<Product>();

            _mockRepo
                .Setup(r => r.GetAllAsync(1, 10))
                .ReturnsAsync((products, 0));

            // Act

            var result =
                await _service.GetAllAsync(1, 10);

            // Assert

            Assert.Empty(result.Products);

            Assert.Equal(0, result.TotalCount);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange

            var product = new Product
            {
                Id = 1,
                Name = "Chair",
                Description = "Wooden chair",
                Price = 5000,
                Stock = 10,
                Category = "Furniture"
            };

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act

            var result =
                await _service.GetByIdAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                "Chair",
                result!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            // Act

            var result =
                await _service.GetByIdAsync(1);

            // Assert

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProductSuccessfully()
        {
            // Arrange

            var dto = new CreateProductDTO
            {
                Name = "Sofa",
                Description = "Luxury sofa",
                Price = 20000,
                Stock = 5,
                Category = "Furniture",
                ImageUrl = "/images/sofa.jpg"
            };

            var product = new Product
            {
                Id = 1,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepo
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);

            // Act

            var result =
                await _service.CreateAsync(dto);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                dto.Name,
                result.Name);

            _mockRepo.Verify(
                r => r.AddAsync(It.IsAny<Product>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldBuildCorrectImageUrl()
        {
            // Arrange

            var dto = new CreateProductDTO
            {
                Name = "Chair",
                Description = "Modern chair",
                Price = 1000,
                Stock = 2,
                Category = "Furniture",
                ImageUrl = "/images/chair.jpg"
            };

            var product = new Product
            {
                Id = 1,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl
            };

            _mockRepo
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);

            // Act

            var result =
                await _service.CreateAsync(dto);

            // Assert

            Assert.Contains(
                "https://localhost:7210",
                result.ImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProductSuccessfully()
        {
            // Arrange

            var product = new Product
            {
                Id = 1,
                Name = "Old Chair",
                Description = "Old",
                Price = 3000,
                Stock = 5,
                Category = "Furniture"
            };

            var dto = new UpdateProductDTO
            {
                Name = "New Chair",
                Description = "Updated",
                Price = 5000,
                Stock = 10,
                Category = "Furniture",
                ImageUrl = "/images/chair.jpg"
            };

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act

            await _service.UpdateAsync(1, dto);

            // Assert

            Assert.Equal(
                "New Chair",
                product.Name);

            _mockRepo.Verify(
                r => r.UpdateAsync(product),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            var dto = new UpdateProductDTO();

            // Act & Assert

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAsync(1, dto));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProductSuccessfully()
        {
            // Arrange

            var product = new Product
            {
                Id = 1,
                Name = "Chair"
            };

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act

            await _service.DeleteAsync(1);

            // Assert

            _mockRepo.Verify(
                r => r.SoftDeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            // Act & Assert

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAsync(1));
        }

        [Fact]
        public async Task ReduceStockAsync_ShouldReduceStockSuccessfully()
        {
            // Arrange

            var product = new Product
            {
                Id = 1,
                Name = "Chair",
                Stock = 10
            };

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act

            await _service.ReduceStockAsync(1, 2);

            // Assert

            Assert.Equal(8, product.Stock);

            _mockRepo.Verify(
                r => r.UpdateAsync(product),
                Times.Once);
        }

        [Fact]
        public async Task ReduceStockAsync_ShouldThrowException_WhenStockInsufficient()
        {
            // Arrange

            var product = new Product
            {
                Id = 1,
                Name = "Chair",
                Stock = 1
            };

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => _service.ReduceStockAsync(1, 5));
        }

        [Fact]
        public async Task ReduceStockAsync_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            // Act & Assert

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.ReduceStockAsync(1, 5));
        }

        [Fact]
        public async Task RestockProductAsync_ShouldIncreaseStockSuccessfully()
        {
            // Arrange

            var product = new Product
            {
                Id = 1,
                Name = "Chair",
                Stock = 5
            };

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act

            await _service.RestockProductAsync(1, 10);

            // Assert

            Assert.Equal(15, product.Stock);

            _mockRepo.Verify(
                r => r.UpdateAsync(product),
                Times.Once);
        }

        [Fact]
        public async Task RestockProductAsync_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange

            _mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            // Act & Assert

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.RestockProductAsync(1, 10));
        }
    }
}