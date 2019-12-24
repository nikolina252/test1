using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Dtos;
using ZadatakTest.Models;
using ZadatakTest.Services;

namespace ZadatakTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private IArticleRepository _articleRepository;
        private ICategoryRepository _categoryRepository;
        private IUserRepository _userRepository;
        public ArticlesController(IArticleRepository articleRepository, 
            ICategoryRepository categoryRepository,IUserRepository userRepository)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }
        //api/articales
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticleDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetArticles()
        {
            var articles = _articleRepository.GetArticles().ToList();
            

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articlesDto = new List<ArticleDto>();
            foreach (var article in articles)
            {
                var category = _categoryRepository.GetCategory(article.CategoryId);
                var owner = _userRepository.GetUser(article.OwnerId);

                articlesDto.Add(new ArticleDto
                {
                    Id = article.Id,
                    Name = article.Name,
                    Price = article.Price,
                    ShortDescription = article.ShortDescription,
                    FullDescription = article.FullDescription,
                    DateOfPublication = article.DateOfPublication,
                    CategoryName = category.Name,
                    OwnerName = owner.FirstName +" "+ owner.LastName


                }) ; 
            }
            return Ok(articlesDto);
        }
        //api/articles/articleId
        [HttpGet("{articleId}", Name = "GetArticle")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ArticleDto))]
        public IActionResult GetArticle(int articleId)
        {
            if (!_articleRepository.ArticleExists(articleId))
                return NotFound();
            var article = _articleRepository.GetArticle(articleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategory(article.CategoryId);
            var owner = _userRepository.GetUser(article.OwnerId);


            var articleDto = new ArticleDto()
            {
                Id = article.Id,
                Name = article.Name,
                Price = article.Price,
                ShortDescription = article.ShortDescription,
                FullDescription = article.FullDescription,
                DateOfPublication = article.DateOfPublication,
                CategoryName = category.Name,
                OwnerName = owner.FirstName + " " + owner.LastName
            };
            return Ok(articleDto);
        }
        //create
        //api/articles
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Article))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateArticle([FromBody]Article articleToCreate)
        {
            if (articleToCreate == null)
                return BadRequest(ModelState);

            articleToCreate.DateOfPublication = DateTime.Now;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_articleRepository.CreateArticle(articleToCreate))
            {
                ModelState.AddModelError("", $"Nešto nije uredu prilikom kriranja artika!!!");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetArticle", new { articleId = articleToCreate.Id }, articleToCreate);
        }
        //update
        //api/articles/articleId
        [HttpPatch("{articleId}")]
        [ProducesResponseType(204)] //NoContent
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpadateArticle(int articleId, [FromBody]Article updateArticleInfo)
        {
            if (updateArticleInfo == null)
                return BadRequest(ModelState);

            if (articleId != updateArticleInfo.Id)
                return BadRequest(ModelState);

            if (!_articleRepository.ArticleExists(articleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_articleRepository.UpdateArticle(updateArticleInfo))
            {
                ModelState.AddModelError("", $"Nešto nije uredu prilikom ažuriranja artika!!!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //delete
        //api/articles/articleId
        [HttpDelete("{articleId}")]
        [ProducesResponseType(204)] //NoContent
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteArticle(int articleId)
        {
            if (!_articleRepository.ArticleExists(articleId))
                return NotFound();
            var arcicleToDelete = _articleRepository.GetArticle(articleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_articleRepository.DeleteArticle(arcicleToDelete))
            {
                ModelState.AddModelError("", $"Nešto nije uredu prilikom brisanja artikla!!!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
