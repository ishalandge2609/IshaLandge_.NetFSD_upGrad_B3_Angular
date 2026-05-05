using ContactService.Data;
using ContactService.Models;

namespace ContactService.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactDbContext _context;

        public ContactRepository(ContactDbContext context)
        {
            _context = context;
        }

        public List<Contact> GetAll()
        {
            return _context.Contacts.ToList();
        }

        public Contact GetById(int id)
        {
            return _context.Contacts.Find(id);
        }

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
            var contact = _context.Contacts.Find(id);

            if (contact == null)
                return false;

            _context.Contacts.Remove(contact);
            _context.SaveChanges();

            return true;
        }
    }
}
