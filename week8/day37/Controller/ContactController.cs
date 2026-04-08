using ContactManagementAPI.DataAccess;
using ContactManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _repo;

        public ContactController(IContactRepository repo)
        {
            _repo = repo;
        }


        // GET ALL
        [HttpGet]
        public IActionResult GetAllContacts()
        {
            return Ok(_repo.GetAll());
        }



        // GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var contact = _repo.GetById(id);

            if (contact == null) { 
                return NotFound();
            }
            else
            {
                return Ok(contact);
            }
           
        }

        // CREATE
        [HttpPost]
        public IActionResult Create(ContactInfo contact)
        {
            var created = _repo.Add(contact);
            if (contact == null)
            {
                return BadRequest();
            }

            else {
                return CreatedAtAction(nameof(GetById),
                    new { id = created.ContactId },
                    created);
            }
           
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult Update(int id, ContactInfo contact)
        {
            var updated = _repo.Update(id, contact);

            if (updated == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(updated);
            }
                

           
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _repo.Delete(id);

            if (!result)
            {
                return NotFound();
            }
            else
            {
                return Ok("Deleted Successfully");
            }
               

            
        }
    }
}
