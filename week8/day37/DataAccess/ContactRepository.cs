using ContactManagementAPI.Models;

namespace ContactManagementAPI.DataAccess
{
    public class ContactRepository : IContactRepository
    {
        // Static List (Fake Database)
        public static List<ContactInfo> contacts = new List<ContactInfo>()//contactInfo is the model class  and contacts is the variable name which is a list of contactInfo objects.
        {

       

    new ContactInfo
    {
        ContactId = 1,
        FirstName = "Isha",
        LastName = "Landge",
        EmailId = "isha@gmail.com",
        MobileNo = 9876543210,
        Designation = "Developer",
        CompanyId = 1,
        DepartmentId = 101
    },
    new ContactInfo
    {
        ContactId = 2,
        FirstName = "Rahul",
        LastName = "Sharma",
        EmailId = "rahul@gmail.com",
        MobileNo = 9123456780,
        Designation = "Tester",
        CompanyId = 2,
        DepartmentId = 102
    },
    new ContactInfo
    {
        ContactId = 3,
        FirstName = "Sneha",
        LastName = "Patil",
        EmailId = "sneha@gmail.com",
        MobileNo = 9988776655,
        Designation = "Manager",
        CompanyId = 1,
        DepartmentId = 103
    }
};
    

        public ContactInfo Add(ContactInfo contact)//Add(ContactInfo contact) contact info type and contact is the variable name
        {
            contact.ContactId = contacts.Count + 1; // Auto ID
            contacts.Add(contact);
            return contact;
        }
        

        public bool Delete(int id)
        {
            var contact = contacts.Find(c => c.ContactId == id);

            if (contact == null)
            {
                return false;
            }
            else
            {
                contacts.Remove(contact);
                return true;
            }
                

           
        }

        public List<ContactInfo> GetAll()
        {
            return contacts ;
        }

        public ContactInfo GetById(int id)
        {
            return contacts.FirstOrDefault(c => c.ContactId == id);
        }

        public ContactInfo Update(int id, ContactInfo contact)
        {
            var existing = contacts.Find(c => c.ContactId == id);

            if (existing == null)
            {
                return null;
            }
            else
            {
                existing.FirstName = contact.FirstName;
                existing.LastName = contact.LastName;
                existing.EmailId = contact.EmailId;
                existing.MobileNo = contact.MobileNo;
                existing.Designation = contact.Designation;
                existing.CompanyId = contact.CompanyId;
                existing.DepartmentId = contact.DepartmentId;

                return existing;
            }

            
        }
    }
    }

