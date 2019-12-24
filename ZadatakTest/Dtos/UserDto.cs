using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZadatakTest.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Balance { get; set; }
    }
}
