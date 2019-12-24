using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Models;

namespace ZadatakTest.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private TestDbContext _categoryContext;
        public CategoryRepository(TestDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }
        public bool CategoryExists(int categoryId)
        {
            return _categoryContext.Categories.Any(c => c.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _categoryContext.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _categoryContext.Remove(category);
            return Save();
        }

        public ICollection<Article> GetArticlesForCategory(int categoryId)
        {
            return _categoryContext.Articles.Where(c => c.CategoryId == categoryId).ToList();
        }

        public ICollection<Category> GetCategories()
        {
            return _categoryContext.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public bool IsDuplicateCategoryName(int categoryId, string categoryName)
        {
            var category = _categoryContext.Categories.Where(c => c.Name.Trim().ToUpper() == categoryName.Trim().ToUpper()
                      && c.Id != categoryId).FirstOrDefault();
            return category == null ? false : true;
        }

        public bool Save()
        {
            var saved = _categoryContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _categoryContext.Update(category);
                return Save();
        }
    }
}
