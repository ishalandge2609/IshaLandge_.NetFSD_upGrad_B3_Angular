using AuthService.Models;

namespace AuthService.Services
{
    public interface IAuthService
    {
        User Register(User user);

        string Login(string email, string password);

        List<User> GetAllUsers();
    }
}
