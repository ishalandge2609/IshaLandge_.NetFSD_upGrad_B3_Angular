using Microsoft.AspNetCore.Mvc;
using MyFirstMVCApp.Models;

namespace MyFirstMVCApp.Controllers
{ 

public class ProductController : Controller


{
    public static List<Product> products = new List<Product>
    {
        new Product { ProductId = 1, ProductName = "Laptop", Category = "Electronics", Price = 60000 },
        new Product { ProductId = 2, ProductName = "Mobile", Category = "Electronics", Price = 25000},
        new Product { ProductId = 3, ProductName = "Headphones", Category = "Accessories", Price = 2000},
        new Product { ProductId = 4, ProductName = "Keyboard", Category = "Accessories", Price = 1500},
        new Product { ProductId = 5, ProductName = "Chair", Category = "Furniture", Price = 5000 }
    };

    public IActionResult Index()
    {
        return View(products);
    }

    public IActionResult Details(int id)
    {
        Product empObj = products.FirstOrDefault(item => item.ProductId == id);
        return View(empObj);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product p)
    {
        if (ModelState.IsValid)
        {
            products.Add(p);
            return RedirectToAction("Index");
        }
        return View(p);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = products.FirstOrDefault(p => p.ProductId == id);
        return View(product);
    }

    [HttpPost]
        // EDIT (POST)
        [HttpPost]
        public IActionResult Edit(Product p)
        {
            var product = products.FirstOrDefault(x => x.ProductId == p.ProductId);

            if (product != null && ModelState.IsValid)
            {
                product.ProductName = p.ProductName;
                product.Price = p.Price;
                product.Category = p.Category;
            }

            return RedirectToAction("Index");
        }

        // DELETE (GET - Show confirmation page)
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(x => x.ProductId == id);
            return View(product);
        }

        // DELETE (POST - Actually delete)
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = products.FirstOrDefault(x => x.ProductId == id);

            if (product != null)
            {
                products.Remove(product);
            }

            return RedirectToAction("Index");
        }
    }
}
