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
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //api/users
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usersDto = new List<UserDto>();
            foreach (var user in users)
            {
                usersDto.Add(new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    TelephoneNumber = user.TelephoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    City = user.City,
                    Country = user.Country,
                    Balance = user.Balance

                });
            }
            return Ok(usersDto);
        }
        //api/users/userId
        [HttpGet("{userId}", Name = "GetUser")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public IActionResult GetUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();

            var user = _userRepository.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                TelephoneNumber = user.TelephoneNumber,
                DateOfBirth = user.DateOfBirth,
                City = user.City,
                Country = user.Country,
                Balance = user.Balance
            };
            return Ok(userDto);
        }

        //create
        //api/users
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateUser([FromBody]User userToCreate)
        {
            if (userToCreate == null)

                return BadRequest(ModelState);

            var email =  _userRepository.GetUsers()
                .Where(e => e.Email.Trim().ToUpper()
                == userToCreate.Email.Trim().ToUpper()).FirstOrDefault();
            if (email != null)
            {
                ModelState.TryAddModelError("", $"Email {userToCreate.Email} već postoji!");
                return StatusCode(422, $"Email {userToCreate.Email} već postoji!");
            }

            userToCreate.Balance = 1000.00M;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            if (!_userRepository.CreateUser(userToCreate))
            {
                ModelState.AddModelError("", "Nešto nije uredu prilikom kreiranja!!!");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetUser", new { userId = userToCreate.Id}, userToCreate);
        }
        //update
        //api/users/userId
        [HttpPatch("{userId}")]
        [ProducesResponseType(204)] //NoContent
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpadateUser(int userId, [FromBody]User updateUserInfo)
        {
            if (updateUserInfo == null)
                return BadRequest(ModelState);

            if (userId != updateUserInfo.Id)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(userId))
                return NotFound();

            if (_userRepository.EniqueEmail(userId, updateUserInfo.Email))
            {
                ModelState.AddModelError("", $"Email {updateUserInfo.Email} već postoji! ");
                return StatusCode(422, $"Email {updateUserInfo.Email} već postoji! ");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.UpadateUser(updateUserInfo))
            {
                ModelState.AddModelError("", "Nešto nije uredu prilikom kreiranja!!!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //delete
        //api/users/userId
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)] //NoContent
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();
            var userToDelete = _userRepository.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", $"Nešto nije uredu prilikom brisanja korisnika!!!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
