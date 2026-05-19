using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.DTOs;
using OrderService.API.Services;
using System.Security.Claims;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Creates a new order
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateOrderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Get logged-in user ID from JWT token
                var userId = int.Parse(
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier)!);

                // Extract JWT token from request header
                var token = HttpContext.Request.Headers[
                    "Authorization"]
                    .ToString()
                    .Replace("Bearer ", "");

                var order = await _orderService
                    .CreateOrderAsync(
                        userId,
                        dto,
                        token);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = order.OrderId },
                    order);
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message
                });
            }
        }

        // Fetch order by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService
                .GetByIdAsync(id);

            if (order == null)
            {
                return NotFound(new
                {
                    error = "Order not found"
                });
            }

            return Ok(order);
        }

        // Deletes an order
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

            var isAdmin = User.IsInRole("Admin");

            await _orderService.DeleteAsync(
                id,
                userId,
                isAdmin);

            return Ok(new
            {
                message = "Order deleted successfully"
            });
        }

        // Cancels an existing order
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

            var isAdmin = User.IsInRole("Admin");

            // Extract JWT token from request header
            var token = HttpContext.Request.Headers[
                "Authorization"]
                .ToString()
                .Replace("Bearer ", "");

            await _orderService.CancelOrderAsync(
                id,
                userId,
                isAdmin,
                token);

            return Ok(new
            {
                success = true,
                message = "Order cancelled successfully"
            });
        }

        // Returns orders of logged-in user
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

            var orders = await _orderService
                .GetOrdersByUserIdAsync(userId);

            return Ok(orders);
        }

        // Returns all orders (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService
                .GetAllOrdersAsync();

            return Ok(orders);
        }
    }
}