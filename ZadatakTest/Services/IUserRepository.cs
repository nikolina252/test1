using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Models;

namespace ZadatakTest.Services
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int userId);
        bool UserExists(int userId);
        bool EniqueEmail(int userId, string userEmail);

        bool CreateUser(User user);
        bool UpadateUser(User user);
        bool DeleteUser(User user);
        bool Save();



    }
}
