using CategoryService.Models;
using CategoryService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CategoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllCategories());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _service.GetCategoryById(id);

            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            var result = _service.AddCategory(category);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Category category)
        {
            var result = _service.UpdateCategory(id, category);

            if (result == null)
                return NotFound("Category not found");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _service.DeleteCategory(id);

            if (!deleted)
                return NotFound("Category not found");

            return Ok("Category deleted successfully");
        }

    }
}
