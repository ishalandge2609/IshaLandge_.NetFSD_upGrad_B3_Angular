using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using System.Text.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public OrdersController(HttpClient httpClient)//constructor
        {
            _httpClient = httpClient;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> CreateOrder(int productId)
        {
            var response = await _httpClient.GetAsync(
                $"https://localhost:7002/api/Product/{productId}");

            if (!response.IsSuccessStatusCode)
                return BadRequest("Product not found");

            var jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var productData = JsonSerializer.Deserialize<Product>(jsonData, options);

            var order = new
            {
                OrderId = 1,
                Product = productData
            };

            return Ok(order);
        }
    }
}