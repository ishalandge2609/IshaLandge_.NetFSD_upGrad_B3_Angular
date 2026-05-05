using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEZ.WebAPI.DTOs;
using ShopEZ.WebAPI.Helpers;
using ShopEZ.WebAPI.Services;

namespace ShopEZ.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        // GET PRODUCTS (Pagination + Search + Filter)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryDTO query)
        {
            var result = await _service.GetProductsAsync(query);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Products fetched successfully",
                Data = result,
                StatusCode = 200
            });
        }

        // GET PRODUCT BY ID (Public)
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Product with ID {id} not found",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse<ProductResponseDTO>
            {
                Success = true,
                Message = "Product retrieved successfully",
                Data = product,
                StatusCode = 200
            });
        }

        //  CREATE PRODUCT (Admin Only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductCreateDTO dto)
        {
            var result = await _service.CreateAsync(dto);

            return StatusCode(201, new ApiResponse<ProductResponseDTO>
            {
                Success = true,
                Message = "Product created successfully",
                Data = result,
                StatusCode = 201
            });
        }

        // UPDATE PRODUCT (Admin Only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, ProductCreateDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Product with ID {id} not found",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = $"Product with ID {id} updated successfully",
                StatusCode = 200
            });
        }

        // DELETE PRODUCT (Admin Only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Product with ID {id} not found",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = $"Product with ID {id} deleted successfully",
                StatusCode = 200
            });
        }
    }
}