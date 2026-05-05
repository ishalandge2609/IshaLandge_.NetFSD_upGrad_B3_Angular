using CategoryService.Models;
using CategoryService.Repository;

namespace CategoryService.Services
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryServiceImpl(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public List<Category> GetAll() => _repo.GetAll();

        public Category GetById(int id) => _repo.GetById(id);

        public Category Add(Category category) => _repo.Add(category);

        public Category Update(Category category) => _repo.Update(category);

        public bool Delete(int id) => _repo.Delete(id);
    }
}