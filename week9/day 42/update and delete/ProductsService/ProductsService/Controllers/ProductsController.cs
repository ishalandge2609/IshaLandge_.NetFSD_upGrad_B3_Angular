using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Models;

namespace ProductsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
            private static List<Product> products = new List<Product>()//demo data for testing the API methods
        {
            new Product { Id = 1, Name = "Laptop", Price= 75000, Category = "Electronics"},
            new Product { Id = 2, Name = "Mobile", Price= 65000, Category = "Electronics"},
            new Product { Id = 3, Name = "Tablet", Price= 55000, Category = "Electronics"}
        };


            [HttpGet]//get all products
        public IActionResult GetProducts()
            {
                return Ok(products);
            }

            [HttpGet("{id}")]//get product details by id
        public IActionResult GetProductById(int id) //api/products/1  
        {
                var product = products.Find(item => item.Id == id);//find product by product id

            if (product == null)//if product is not found
            {
                    return NotFound("Requested product does not exists");//404 status code
            }
                else
                {
                    return Ok(product);//200 status code with product details in response body
            }
            }

            [HttpPost]//add new product details to the server
        public IActionResult AddProduct(Product product)
            {
                products.Add(product);//add new product to the list

            // Customize the response :  data, status codes 
            return Ok(product);//200 status code with added product details in response body
        }


            [HttpPut("{id}")]//update existing product details in the server
        public IActionResult UpdateProduct(int id, Product product)//int id is used to find the existing product and Product product is used to update the existing product details
        {
                var oldProduct = products.Find(item => item.Id == id);//find existing product by product id

            if (oldProduct == null)//if product is not found
                {
                    return NotFound("Requested product does not exists");//404 status code  
                }
            else//if product is found then update the existing product details with new product details
            {
                    oldProduct.Name = product.Name;

                    oldProduct.Category = product.Category;
                    oldProduct.Price = product.Price;
                    return Ok(new { updatedProduct = oldProduct, status = "Product details are updated successfully in server..!" });
                }
            }

            [HttpDelete("{id}")]//delete existing product details from the server
        public IActionResult DeleteProduct(int id)//id is used to find the existing product details and delete it from the server
        {
                var product = products.Find(item => item.Id == id);//find existing product by product id

            if (product == null)//if product is not found
            {
                    return NotFound("Requested product does not exists");//404 status code
            }
                else
                {
                    products.Remove(product);//remove the existing product details from the list
                return Ok(new { product, status = "Product details are deleted successfully in server..!" });
                }
            }
        }
    }
