using CategoryService.Models;
using CategoryService.Repository;

namespace CategoryService.Services
{
    public class CategoryServiceManager:ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryServiceManager(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public List<Category> GetAllCategories()
        {
            return _repository.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            return _repository.GetById(id);
        }

        public Category AddCategory(Category category)
        {
            return _repository.Add(category);
        }

        public Category UpdateCategory(int id, Category category)
        {
            var existing = _repository.GetById(id);

            if (existing == null)
                return null;

            existing.CategoryName = category.CategoryName;
            existing.Description = category.Description;

            return _repository.Update(existing);
        }

        public bool DeleteCategory(int id)
        {
            return _repository.Delete(id);
        }
    }
}
