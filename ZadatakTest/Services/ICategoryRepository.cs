using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Models;

namespace ZadatakTest.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Article> GetArticlesForCategory(int categoryId);
        bool CategoryExists(int categoryId);
        bool IsDuplicateCategoryName(int categoryId, string categoryName);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
