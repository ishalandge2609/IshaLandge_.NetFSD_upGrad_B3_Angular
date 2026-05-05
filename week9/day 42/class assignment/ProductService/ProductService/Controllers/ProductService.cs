using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Model;


namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = new Product()
            {
                Id = id,
                Name = "Smart Phone",
                Price = 60000,
                Category = "Electronics"
            };

            return Ok(product);
        }

    }
}