using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using OrderService.API.Controllers;
using OrderService.API.Services;
using OrderService.API.DTOs;

namespace OrderService.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;

        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();

            _controller = new OrderController(
                _mockOrderService.Object);
        }

        private void SetupUserClaims(
            int userId,
            string role = "User")
        {
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    userId.ToString()),

                new Claim(
                    ClaimTypes.Role,
                    role)
            };

            var identity = new ClaimsIdentity(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext =
                new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                };

            _controller.HttpContext
                .Request
                .Headers["Authorization"] =
                "Bearer fake-jwt-token";
        }

        [Fact]
        public async Task Create_ValidOrder_ReturnsCreatedAtAction()
        {
            // Arrange
            SetupUserClaims(1);

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

            var response = new OrderResponseDTO
            {
                OrderId = 1,
                UserId = 1,
                TotalAmount = 5000
            };

            _mockOrderService
                .Setup(s => s.CreateOrderAsync(
                    1,
                    dto,
                    "fake-jwt-token"))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdResult =
                Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Create_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState
                .AddModelError("Items", "Items required");

            var dto = new CreateOrderDTO();

            // Act
            var result = await _controller.Create(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ExistingOrder_ReturnsOk()
        {
            // Arrange
            var response = new OrderResponseDTO
            {
                OrderId = 1,
                UserId = 1
            };

            _mockOrderService
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetById_InvalidOrder_ReturnsNotFound()
        {
            // Arrange
            _mockOrderService
                .Setup(s => s.GetByIdAsync(999))
                .ReturnsAsync((OrderResponseDTO?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsOk()
        {
            // Arrange
            SetupUserClaims(1, "Admin");

            _mockOrderService
                .Setup(s => s.DeleteAsync(
                    1,
                    1,
                    true))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task CancelOrder_ReturnsOk()
        {
            // Arrange
            SetupUserClaims(1);

            _mockOrderService
                .Setup(s => s.CancelOrderAsync(
                    1,
                    1,
                    false,
                    "fake-jwt-token"))
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _controller.CancelOrder(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetMyOrders_ReturnsOk()
        {
            // Arrange
            SetupUserClaims(1);

            var orders = new List<OrderResponseDTO>
            {
                new OrderResponseDTO
                {
                    OrderId = 1,
                    UserId = 1
                }
            };

            _mockOrderService
                .Setup(s => s.GetOrdersByUserIdAsync(1))
                .ReturnsAsync(orders);

            // Act
            var result =
                await _controller.GetMyOrders();

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsOk()
        {
            // Arrange
            SetupUserClaims(1, "Admin");

            var orders = new List<OrderResponseDTO>
            {
                new OrderResponseDTO
                {
                    OrderId = 1,
                    UserId = 1
                }
            };

            _mockOrderService
                .Setup(s => s.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result =
                await _controller.GetAllOrders();

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }
    }
}