using Microsoft.Extensions.Logging;
using Moq;
using OrderService.API.DTOs;
using OrderService.API.Exceptions;
using OrderService.API.Models;
using OrderService.API.Repositories;
using OrderService.API.Services;
using Xunit;

namespace OrderService.Tests.Services
{
    public class OrdersServiceTests
    {
        private readonly Mock<IOrderRepository> _mockRepo;

        private readonly Mock<ProductServiceClient> _mockProductClient;

        private readonly Mock<ILogger<OrdersService>> _mockLogger;

        private readonly OrdersService _service;

        public OrdersServiceTests()
        {
            _mockRepo =
                new Mock<IOrderRepository>();

            _mockProductClient =
                new Mock<ProductServiceClient>(
                    new HttpClient());

            _mockLogger =
                new Mock<ILogger<OrdersService>>();

            _service = new OrdersService(
                _mockRepo.Object,
                _mockProductClient.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrderSuccessfully()
        {
            // Arrange

            var dto = new CreateOrderDTO
            {
                Items = new List<CreateOrderItemDTO>
                {
                    new CreateOrderItemDTO
                    {
                        ProductId = 1,
                        Quantity = 2
                    }
                }
            };

            var product = new ProductInfo
            {
                Id = 1,
                Name = "Laptop",
                Price = 50000,
                Stock = 10
            };

            _mockProductClient
                .Setup(x => x.GetProductAsync(1, "token"))
                .ReturnsAsync(product);

            _mockProductClient
                .Setup(x => x.DeductStockAsync(1, 2, "token"))
                .Returns(Task.CompletedTask);

            _mockRepo
                .Setup(x => x.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order o) => o);

            // Act

            var result =
                await _service.CreateOrderAsync(
                    1,
                    dto,
                    "token");

            // Assert

            Assert.NotNull(result);

            Assert.Equal(100000, result.TotalAmount);

            Assert.Single(result.Items);

            _mockRepo.Verify(
                x => x.AddAsync(It.IsAny<Order>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange

            var dto = new CreateOrderDTO
            {
                Items = new List<CreateOrderItemDTO>
                {
                    new CreateOrderItemDTO
                    {
                        ProductId = 1,
                        Quantity = 2
                    }
                }
            };

            _mockProductClient
                .Setup(x => x.GetProductAsync(1, "token"))
                .ReturnsAsync((ProductInfo?)null);

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => _service.CreateOrderAsync(
                    1,
                    dto,
                    "token"));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowException_WhenStockInsufficient()
        {
            // Arrange

            var dto = new CreateOrderDTO
            {
                Items = new List<CreateOrderItemDTO>
                {
                    new CreateOrderItemDTO
                    {
                        ProductId = 1,
                        Quantity = 5
                    }
                }
            };

            var product = new ProductInfo
            {
                Id = 1,
                Name = "Laptop",
                Price = 50000,
                Stock = 2
            };

            _mockProductClient
                .Setup(x => x.GetProductAsync(1, "token"))
                .ReturnsAsync(product);

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => _service.CreateOrderAsync(
                    1,
                    dto,
                    "token"));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange

            var order = new Order
            {
                OrderId = 1,
                UserId = 1,
                TotalAmount = 5000,
                Status = "Order Placed",
                OrderItems = new List<OrderItem>()
            };

            _mockRepo
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(order);

            // Act

            var result =
                await _service.GetByIdAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(1, result!.OrderId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenOrderNotFound()
        {
            // Arrange

            _mockRepo
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Order?)null);

            // Act

            var result =
                await _service.GetByIdAsync(1);

            // Assert

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteOrderSuccessfully()
        {
            // Arrange

            var order = new Order
            {
                OrderId = 1,
                UserId = 1
            };

            _mockRepo
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(order);

            // Act

            await _service.DeleteAsync(
                1,
                1,
                false);

            // Assert

            _mockRepo.Verify(
                x => x.SoftDeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenOrderNotFound()
        {
            // Arrange

            _mockRepo
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Order?)null);

            // Act & Assert

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAsync(
                    1,
                    1,
                    false));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowUnauthorizedException()
        {
            // Arrange

            var order = new Order
            {
                OrderId = 1,
                UserId = 2
            };

            _mockRepo
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(order);

            // Act & Assert

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.DeleteAsync(
                    1,
                    1,
                    false));
        }

        [Fact]
        public async Task GetOrdersByUserIdAsync_ShouldReturnOrders()
        {
            // Arrange

            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    UserId = 1,
                    TotalAmount = 1000,
                    Status = "Order Placed",
                    OrderItems = new List<OrderItem>()
                }
            };

            _mockRepo
                .Setup(x => x.GetOrdersByUserIdAsync(1))
                .ReturnsAsync(orders);

            // Act

            var result =
                await _service.GetOrdersByUserIdAsync(1);

            // Assert

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnAllOrders()
        {
            // Arrange

            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    UserId = 1,
                    TotalAmount = 5000,
                    Status = "Order Placed",
                    OrderItems = new List<OrderItem>()
                }
            };

            _mockRepo
                .Setup(x => x.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act

            var result =
                await _service.GetAllOrdersAsync();

            // Assert

            Assert.Single(result);
        }

        [Fact]
        public async Task CancelOrderAsync_ShouldCancelOrderSuccessfully()
        {
            // Arrange

            var order = new Order
            {
                OrderId = 1,
                UserId = 1,
                Status = "Order Placed",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = 1,
                        Quantity = 2
                    }
                }
            };

            _mockRepo
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(order);

            _mockProductClient
                .Setup(x => x.RestockProductAsync(
                    1,
                    2,
                    "token"))
                .Returns(Task.CompletedTask);

            // Act

            await _service.CancelOrderAsync(
                1,
                1,
                false,
                "token");

            // Assert

            Assert.Equal(
                "Cancelled",
                order.Status);

            _mockRepo.Verify(
                x => x.CancelOrderAsync(order),
                Times.Once);
        }

        [Fact]
        public async Task CancelOrderAsync_ShouldThrowException_WhenOrderNotFound()
        {
            // Arrange

            _mockRepo
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Order?)null);

            // Act & Assert

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CancelOrderAsync(
                    1,
                    1,
                    false,
                    "token"));
        }

        [Fact]
        public async Task CancelOrderAsync_ShouldThrowException_WhenAlreadyCancelled()
        {
            // Arrange

            var order = new Order
            {
                OrderId = 1,
                UserId = 1,
                Status = "Cancelled"
            };

            _mockRepo
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(order);

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => _service.CancelOrderAsync(
                    1,
                    1,
                    false,
                    "token"));
        }

        [Fact]
        public async Task CancelOrderAsync_ShouldThrowUnauthorizedException()
        {
            // Arrange

            var order = new Order
            {
                OrderId = 1,
                UserId = 2,
                Status = "Order Placed"
            };

            _mockRepo
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(order);

            // Act & Assert

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.CancelOrderAsync(
                    1,
                    1,
                    false,
                    "token"));
        }
    }
}