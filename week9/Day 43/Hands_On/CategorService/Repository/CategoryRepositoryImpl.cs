using CategoryService.Data;
using CategoryService.Models;

namespace CategoryService.Repository
{
    public class CategoryRepositoryImpl : ICategoryRepository
    {
        private readonly CategoryDbContext _context;

        public CategoryRepositoryImpl(CategoryDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAll() => _context.Categories.ToList();

        public Category GetById(int id) => _context.Categories.Find(id);

        public Category Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return category;
        }

        public bool Delete(int id)
        {
            var data = _context.Categories.Find(id);
            if (data == null) return false;

            _context.Categories.Remove(data);
            _context.SaveChanges();
            return true;
        }
    }
}