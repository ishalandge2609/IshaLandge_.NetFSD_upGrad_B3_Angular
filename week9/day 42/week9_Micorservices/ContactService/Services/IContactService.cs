using ContactService.Models;

namespace ContactService.Services
{
    public interface IContactService
    {
        List<Contact> GetAllContacts();
        Contact GetContactById(int id);
        Contact AddContact(Contact contact);
        Contact UpdateContact(int id, Contact contact);
        bool DeleteContact(int id);
    }
}
