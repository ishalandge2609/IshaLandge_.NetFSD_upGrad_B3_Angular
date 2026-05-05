using CategoryService.Models;

namespace CategoryService.Repository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category GetById(int id);
        Category Add(Category category);
        Category Update(Category category);
        bool Delete(int id);
    }
}
