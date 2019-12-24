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
    public class CategoriesController : ControllerBase
    {
        private ICategoryRepository _categoryRepository;
        private IUserRepository _userRepository;
        public CategoriesController(ICategoryRepository categoryRepository,IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }
        //api/categories
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name

                });
            }
            return Ok(categoriesDto);
        }
        //api/categories/categoryId
        [HttpGet("{categoryId}", Name = "GetCategory")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();
            var category = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };
            return Ok(categoryDto);
        }

        //create
        //api/categories
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCategory([FromBody]Category categoryToCreate)
        {
            if (categoryToCreate == null)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper()
                == categoryToCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (category != null)
            {
                ModelState.TryAddModelError("", $"Kategorija {categoryToCreate.Name} već postoji!");
                return StatusCode(422, $"Kategorija {categoryToCreate.Name} već postoji!");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CreateCategory(categoryToCreate))
            {
                ModelState.AddModelError("", $"Nešto nije uredu prilikom kriranja kategorije: {categoryToCreate.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { categoryId = categoryToCreate.Id }, categoryToCreate);
        }
        //update
        //api/categories/categoryId
        [HttpPatch("{categoryId}")]
        [ProducesResponseType(204)] //NoContent
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpadateCategory(int categoryId, [FromBody]Category upadtedCategoryInfo)
        {
            if (upadtedCategoryInfo == null)
                return BadRequest(ModelState);

            if (categoryId != upadtedCategoryInfo.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            if (_categoryRepository.IsDuplicateCategoryName(categoryId, upadtedCategoryInfo.Name))
            {
                ModelState.AddModelError("", $"Kategorija {upadtedCategoryInfo.Name} već postoji! ");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.UpdateCategory(upadtedCategoryInfo))
            {
                ModelState.AddModelError("", $"Nešto nije uredu prilikom ažuriranja kategorije: {upadtedCategoryInfo.Name}!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //delete
        //api/categories/categoryId
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)] //NoContent
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();
            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", $"Nešto nije uredu prilikom brisanja kategorije {categoryToDelete.Name} !!!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //GetArticleForCategory
        //api/categories/categoryId/articles
        [HttpGet("{categoryId}/articles")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetArticlesForCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();
            var articles = _categoryRepository.GetArticlesForCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articlesDto = new List<ArticleDto>();
            foreach (var article in articles)
            {
                var category = _categoryRepository.GetCategory(article.CategoryId);
                var owner = _userRepository.GetUser(article.OwnerId);

                articlesDto.Add(new ArticleDto()
                {
                    Id = article.Id,
                    Name = article.Name,
                    Price = article.Price,
                    ShortDescription = article.ShortDescription,
                    FullDescription = article.FullDescription,
                    DateOfPublication = article.DateOfPublication,
                    CategoryName = category.Name,
                    OwnerName = owner.FirstName + " " + owner.LastName
                });
            }

            return Ok(articlesDto);
        }
    }
}
