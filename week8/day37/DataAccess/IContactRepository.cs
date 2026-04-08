using ContactManagementAPI.Models;

namespace ContactManagementAPI.DataAccess
{
    public interface IContactRepository
    {
        List<ContactInfo> GetAll();// list<contactInfo> return type as it will fetch all contacts from the database
        ContactInfo GetById(int id);// contactInfo return type as it will fetch a single contact based on the provided id
        ContactInfo Add(ContactInfo contact);
        ContactInfo Update(int id, ContactInfo contact);
        bool Delete(int id);// bool return type as it will indicate whether the deletion was successful or not
    }
}
