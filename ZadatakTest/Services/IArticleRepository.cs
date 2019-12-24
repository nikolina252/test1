using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Models;

namespace ZadatakTest.Services
{
    public interface IArticleRepository
    {
        ICollection<Article> GetArticles();
        Article GetArticle(int articleId);
        bool ArticleExists(int articleId);

        bool CreateArticle(Article article);
        bool UpdateArticle(Article article);
        bool DeleteArticle(Article article);
        bool Save();
    }
}
