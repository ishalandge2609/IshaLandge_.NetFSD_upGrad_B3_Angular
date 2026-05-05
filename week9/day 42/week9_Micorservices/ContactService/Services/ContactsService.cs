using ContactService.Models;
using ContactService.Repository;

namespace ContactService.Services
{
    public class ContactsService : IContactService
    {
        private readonly IContactRepository _repository;

        public ContactsService(IContactRepository repository)
        {
            _repository = repository;
        }

        public List<Contact> GetAllContacts()
        {
            return _repository.GetAll();
        }

        public Contact GetContactById(int id)
        {
            return _repository.GetById(id);
        }

        public Contact AddContact(Contact contact)
        {
            return _repository.Add(contact);
        }

        public Contact UpdateContact(int id, Contact contact)
        {
            var existing = _repository.GetById(id);

            if (existing == null)
                return null;

            existing.Name = contact.Name;
            existing.Email = contact.Email;
            existing.Phone = contact.Phone;
            existing.CategoryId = contact.CategoryId;

            return _repository.Update(existing);
        }

        public bool DeleteContact(int id)
        {
            return _repository.Delete(id);
        }
    }
}
