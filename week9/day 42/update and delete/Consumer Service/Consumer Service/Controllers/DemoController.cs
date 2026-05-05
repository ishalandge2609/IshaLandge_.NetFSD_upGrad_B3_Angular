using Consumer_Service.Models;
using Consumer_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Consumer_Service.Controllers
{
    public class DemoController: ControllerBase
    {
        private readonly IProductApiService _productApiService;

        public DemoController(IProductApiService productApiService)
        {
            _productApiService = productApiService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(await _productApiService.GetProducts());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok(await _productApiService.GetProductId(id));
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            return Ok(await _productApiService.AddProduct(product));
        }

       
       [HttpPut("{id}")]
       public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            return Ok(await _productApiService.UpdateProduct(id, product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return Ok(await _productApiService.DeleteProduct(id));
        }

    }
}
