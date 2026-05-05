using CategoryService.Models;

namespace CategoryService.Services
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category GetById(int id);
        Category Add(Category category);
        Category Update(Category category);
        bool Delete(int id);
    }
}