using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopEZ.WebAPI.DTOs;
using ShopEZ.WebAPI.Helpers;
using ShopEZ.WebAPI.Services;

namespace ShopEZ.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        // CREATE ORDER
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDTO dto)
        {
            var order = await _service.CreateOrderAsync(dto);

            return StatusCode(201, new ApiResponse<OrderResponseDTO>
            {
                Success = true,
                Message = "Order placed successfully",
                Data = order,
                StatusCode = 201
            });
        }

        //  GET ALL ORDERS (Admin Only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllAsync();

            return Ok(new ApiResponse<IEnumerable<OrderResponseDTO>>
            {
                Success = true,
                Message = "All orders fetched successfully",
                Data = orders,
                StatusCode = 200
            });
        }

        //GET ORDER BY ID (Logged-in user)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Order with ID {id} not found",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse<OrderResponseDTO>
            {
                Success = true,
                Message = "Order retrieved successfully",
                Data = order,
                StatusCode = 200
            });
        }
    }
}
