using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Controllers;
using ProductService.API.Services;
using ProductService.API.DTOs;

namespace ProductService.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;

        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockProductService =
                new Mock<IProductService>();

            _controller =
                new ProductController(
                    _mockProductService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var products = new List<ProductResponseDTO>
            {
                new ProductResponseDTO
                {
                    Id = 1,
                    Name = "Chair",
                    Price = 5000
                }
            };

            _mockProductService
                .Setup(s => s.GetAllAsync(1, 10))
                .ReturnsAsync((products, 1));

            // Act
            var result =
                await _controller.GetAll();

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetById_ExistingProduct_ReturnsOk()
        {
            // Arrange
            var product = new ProductResponseDTO
            {
                Id = 1,
                Name = "Chair",
                Price = 5000
            };

            _mockProductService
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            var result =
                await _controller.GetById(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockProductService
                .Setup(s => s.GetByIdAsync(999))
                .ReturnsAsync((ProductResponseDTO?)null);

            // Act
            var result =
                await _controller.GetById(999);

            // Assert
            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Create_ValidProduct_ReturnsCreated()
        {
            // Arrange
            var dto = new CreateProductDTO
            {
                Name = "Luxury Chair",
                Description = "Modern chair",
                Price = 15000,
                Stock = 5,
                Category = "Furniture"
            };

            var response = new ProductResponseDTO
            {
                Id = 1,
                Name = dto.Name,
                Price = dto.Price
            };

            _mockProductService
                .Setup(s => s.CreateAsync(dto))
                .ReturnsAsync(response);

            // Act
            var result =
                await _controller.Create(dto);

            // Assert
            var createdResult =
                Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsOk()
        {
            // Arrange
            var dto = new UpdateProductDTO
            {
                Name = "Updated Chair",
                Description = "Updated Description",
                Price = 12000,
                Stock = 10,
                Category = "Furniture"
            };

            _mockProductService
                .Setup(s => s.UpdateAsync(1, dto))
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _controller.Update(1, dto);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task ReduceStock_ReturnsOk()
        {
            // Arrange
            var dto = new UpdateStockDTO
            {
                Quantity = 1
            };

            _mockProductService
                .Setup(s => s.ReduceStockAsync(1, 1))
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _controller.ReduceStock(1, dto);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task RestockProduct_ReturnsOk()
        {
            // Arrange
            var dto = new UpdateStockDTO
            {
                Quantity = 10
            };

            _mockProductService
                .Setup(s => s.RestockProductAsync(1, 10))
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _controller.RestockProduct(1, dto);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsOk()
        {
            // Arrange
            _mockProductService
                .Setup(s => s.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _controller.Delete(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }
    }
}