using Microsoft.EntityFrameworkCore;
using Client_Management_System.Models;

namespace Client_Management_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LoginUser> LoginUser { get; set; }
    }
}