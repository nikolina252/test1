using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Dtos;

namespace ZadatakTest.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string FullDescription { get; set; }
        public DateTime DateOfPublication { get; set; }
        public int CategoryId { get; set; }
        public int OwnerId { get; set; }
       
        

    }
}
