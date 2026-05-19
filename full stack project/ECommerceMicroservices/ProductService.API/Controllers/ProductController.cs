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

        // PUBLIC
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int page = 1,
            int pageSize = 10)
        {
            var (products, total) =
                await _productService.GetAllAsync(page, pageSize);

            return Ok(new
            {
                data = products,
                total,
                page,
                pageSize
            });
        }

        // PUBLIC
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Product {id} not found");
            }

            return Ok(new
            {
                data = product
            });
        }

        // ADMIN ONLY
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductDTO dto)
        {
            var product = await _productService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product);
        }

        // ADMIN ONLY
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateProductDTO dto)
        {
            await _productService.UpdateAsync(id, dto);

            return Ok(new
            {
                success = true,
                message = "Product updated successfully"
            });
        }

        // LOGGED-IN USER
        [HttpPut("{id}/stock/reduce")]
        
        public async Task<IActionResult> ReduceStock(
            int id,
            [FromBody] UpdateStockDTO dto)
        {
            await _productService
                .ReduceStockAsync(id, dto.Quantity);

            return Ok(new
            {
                success = true,
                message = "Stock reduced successfully"
            });
        }

        // ADMIN ONLY
        [HttpPut("{id}/stock/restock")]
        
        public async Task<IActionResult> RestockProduct(
            int id,
            [FromBody] UpdateStockDTO dto)
        {
            await _productService
                .RestockProductAsync(id, dto.Quantity);

            return Ok(new
            {
                success = true,
                message = "Product restocked successfully"
            });
        }

        // ADMIN ONLY
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);

            return Ok(new
            {
                success = true,
                message = "Product deleted successfully"
            });
        }
    }
}