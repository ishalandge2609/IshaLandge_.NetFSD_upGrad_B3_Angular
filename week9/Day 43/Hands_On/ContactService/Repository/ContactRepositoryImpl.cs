using ContactService.Data;
using ContactService.Models;

namespace ContactService.Repository
{
    public class ContactRepositoryImpl : IContactRepository
    {
        private readonly ContactDbContext _context;

        public ContactRepositoryImpl(ContactDbContext context)
        {
            _context = context;
        }

        public List<Contact> GetAll() => _context.Contacts.ToList();

        public Contact GetById(int id) => _context.Contacts.Find(id);

        public Contact Add(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return contact;
        }

        public Contact Update(Contact contact)
        {
            _context.Contacts.Update(contact);
            _context.SaveChanges();
            return contact;
        }

        public bool Delete(int id)
        {
            var data = _context.Contacts.Find(id);
            if (data == null) return false;

            _context.Contacts.Remove(data);
            _context.SaveChanges();
            return true;
        }
    }
}
