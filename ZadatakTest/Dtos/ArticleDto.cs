using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Models;

namespace ZadatakTest.Dtos
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string CategoryName { get; set; }
        public string OwnerName { get; set; }

    }
}
