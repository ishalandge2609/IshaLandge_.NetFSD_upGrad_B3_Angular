using ContactService.Models;
using ContactService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _service;

        public ContactsController(IContactService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllContacts());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var contact = _service.GetContactById(id);

            if (contact == null)
                return NotFound("Contact not found");

            return Ok(contact);
        }

        [HttpPost]
        public IActionResult Add(Contact contact)
        {
            var result = _service.AddContact(contact);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Contact contact)
        {
            var result = _service.UpdateContact(id, contact);

            if (result == null)
                return NotFound("Contact not found");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _service.DeleteContact(id);

            if (!deleted)
                return NotFound("Contact not found");

            return Ok("Contact deleted successfully");
        }
    }
}
