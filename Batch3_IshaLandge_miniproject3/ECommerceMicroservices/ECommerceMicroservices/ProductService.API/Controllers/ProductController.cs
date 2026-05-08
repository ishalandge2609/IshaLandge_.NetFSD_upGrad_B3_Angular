using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.DTOs;
using ProductService.API.Services;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var (products, total) = await _productService.GetAllAsync(page, pageSize);
            return Ok(new { data = products, total, page, pageSize });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound($"Product {id} not found");

            return Ok(new { data = product });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
        {
            var product = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO dto)
        {
            await _productService.UpdateAsync(id, dto);
            return NoContent();
        }

        // 🔥🔥 NEW STOCK API (CRITICAL FIX)
        [HttpPut("{id}/stock")]
        [Authorize] // NOT Admin (orders are user-based)
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockDTO dto)
        {
            await _productService.UpdateStockAsync(id, dto.Quantity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}