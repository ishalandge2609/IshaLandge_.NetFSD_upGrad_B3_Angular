using AuthService.Models;

namespace AuthService.Repository
{
    public interface IUserRepository
    {
        User Register(User user);

        User Login(string email, string password);

        List<User> GetAllUsers();
    }
}
