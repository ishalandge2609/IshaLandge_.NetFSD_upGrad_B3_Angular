using Microsoft.AspNetCore.Mvc;
using MyFirstMVCApp.Models;

namespace MyFirstMVCApp.Controllers
{
    
    public class ProductController : Controller
    {// Sample data (Collection)
        private List<Product> products = new List<Product>()
        {
            new Product { ProductId = 1, ProductName = "Laptop", ProductPrice = 50000, Category = "Electronics" },
            new Product { ProductId = 2, ProductName = "shoes", ProductPrice = 2000, Category = "fashion" },
            new Product { ProductId = 3, ProductName = "dress", ProductPrice = 5000, Category = "Fashion" },
            new Product { ProductId = 4, ProductName = "Mixer Grinder", ProductPrice = 3000, Category = "Electronics" },

        };
        


        // INDEX → collection
        public IActionResult Index()
            {
                return View(products);
            }

            // DETAILS → single product
            public IActionResult Details(int id)
            {
                var product = products.FirstOrDefault(p => p.ProductId == id);
                return View(product);
            }
        }
    }
    
    
