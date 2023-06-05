using Microsoft.EntityFrameworkCore;
using UniversityAPI.Models;

namespace UniversityAPI.Data_Context
{
    public class UniversityDBContext : DbContext
    {
        public UniversityDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<University> University { get; set; }
    }
}
