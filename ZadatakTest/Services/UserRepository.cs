using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Models;

namespace ZadatakTest.Services
{
    public class UserRepository : IUserRepository
    {
        private TestDbContext _userContext;
        public UserRepository(TestDbContext userContext)
        {
            _userContext = userContext;
        }
        public bool CreateUser(User user)
        {
            _userContext.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _userContext.Remove(user);
            return Save();
        }

        public bool EniqueEmail(int userId, string userEmail)
        {
            var email = _userContext.Users
                .Where(e => e.Email.Trim().ToUpper()
                 == userEmail.Trim().ToUpper()
                      && e.Id != userId).FirstOrDefault();
            return email == null ? false : true;

        }

        public User GetUser(int userId)
        {
            return _userContext.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _userContext.Users.OrderBy(u => u.FirstName).ToList();
        }

        public bool Save()
        {
            var saved = _userContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpadateUser(User user)
        {
            _userContext.Update(user);
            return Save();
        }

        public bool UserExists(int userId)
        {
            return _userContext.Users.Any(u => u.Id == userId);
        }
    }
}
