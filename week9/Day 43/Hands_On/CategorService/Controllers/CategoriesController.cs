using CategoryService.Models;
using CategoryService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CategoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Get(int id)
        {
            var data = _service.GetById(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(Category category)
            => Ok(_service.Add(category));

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Category category)
            => Ok(_service.Update(category));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            if (!_service.Delete(id)) return NotFound();
            return Ok();
        }
    }
}