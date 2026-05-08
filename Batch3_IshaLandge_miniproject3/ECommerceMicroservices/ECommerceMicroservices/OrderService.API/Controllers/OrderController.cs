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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var token = HttpContext.Request.Headers["Authorization"]
                    .ToString()
                    .Replace("Bearer ", "");

                var order = await _orderService.CreateOrderAsync(userId, dto, token);

                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound("Order not found");
            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");

            await _orderService.DeleteAsync(id, userId, isAdmin);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }
    }
}