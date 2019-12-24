using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Models;

namespace ZadatakTest.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private TestDbContext _articleContext;
        public ArticleRepository(TestDbContext articleContext)
        {
            _articleContext = articleContext;
        }
        public bool ArticleExists(int articleId)
        {
            return _articleContext.Articles.Any(a => a.Id == articleId);
        }

        public bool CreateArticle(Article article)
        {
            _articleContext.Add(article);
            return Save();
        }

        public bool DeleteArticle(Article article)
        {
            _articleContext.Remove(article);
            return Save();
        }

        public Article GetArticle(int articleId)
        {
            return _articleContext.Articles.Where(a => a.Id == articleId).FirstOrDefault();
        }

        public ICollection<Article> GetArticles()
        {
            return _articleContext.Articles.OrderBy(a => a.Name).ToList();
        }

        public bool Save()
        {
            var saved = _articleContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateArticle(Article article)
        {
            _articleContext.Update(article);
            return Save();
        }
    }
}
