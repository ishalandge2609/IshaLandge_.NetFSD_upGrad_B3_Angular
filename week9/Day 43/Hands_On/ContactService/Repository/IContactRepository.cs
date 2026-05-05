using ContactService.Models;

namespace ContactService.Repository
{
    public interface IContactRepository
    {
        List<Contact> GetAll();
        Contact GetById(int id);
        Contact Add(Contact contact);
        Contact Update(Contact contact);
        bool Delete(int id);
    }
}
