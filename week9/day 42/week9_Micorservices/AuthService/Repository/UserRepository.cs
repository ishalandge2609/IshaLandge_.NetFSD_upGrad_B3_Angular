using AuthService.Data;
using AuthService.Models;

namespace AuthService.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public User Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Login(string email, string password)
        {
            return _context.Users
                .FirstOrDefault(x => x.Email == email && x.Password == password);
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
