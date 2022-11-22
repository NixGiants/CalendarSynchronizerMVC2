using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Schedule> Schedules { get; set; }
    }
}
