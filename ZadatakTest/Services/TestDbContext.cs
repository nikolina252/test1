using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZadatakTest.Models;

namespace ZadatakTest.Services
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
            Database.Migrate();
        }
        public virtual DbSet<User>Users  { get; set; }
        public virtual DbSet<Article>Articles  { get; set; }
        public virtual DbSet<Category>Categories  { get; set; }
    }
}
