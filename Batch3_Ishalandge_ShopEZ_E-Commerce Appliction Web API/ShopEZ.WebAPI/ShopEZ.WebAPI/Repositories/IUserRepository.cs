using ShopEZ.WebAPI.Models;

namespace ShopEZ.WebAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);

        Task AddAsync(User user);
    }
}
