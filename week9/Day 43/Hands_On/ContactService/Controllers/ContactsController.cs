using ContactService.Models;
using ContactService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _service;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactService service, ILogger<ContactsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
       [Authorize]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Fetching all contacts");
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
       [Authorize(Roles = "Admin,User")]
        public IActionResult Get(int id)
        {
            _logger.LogInformation($"Fetching contact with ID: {id}");

            var data = _service.GetById(id);

            if (data == null)
            {
                _logger.LogWarning($"Contact not found: {id}");
                return NotFound();
            }

            return Ok(data);
        }

        [HttpPost]
       [Authorize(Roles = "Admin")]
        public IActionResult Add(Contact contact)
        {
            _logger.LogInformation("Adding new contact");
            return Ok(_service.Add(contact));
        }

        [HttpPut]
       [Authorize(Roles = "Admin")]
        public IActionResult Update(Contact contact)
        {
            _logger.LogInformation($"Updating contact ID: {contact.ContactId}");
            return Ok(_service.Update(contact));
        }

        [HttpDelete("{id}")]
       [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Deleting contact ID: {id}");

            if (!_service.Delete(id))
            {
                _logger.LogWarning($"Delete failed. Contact not found: {id}");
                return NotFound();
            }

            return Ok();
        }
    }
}