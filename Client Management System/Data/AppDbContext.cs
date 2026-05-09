using Microsoft.EntityFrameworkCore;
using Client_Management_System.Models;

namespace Client_Management_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Persons> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persons>()
                .ToTable("persons"); 
        }
    }
}