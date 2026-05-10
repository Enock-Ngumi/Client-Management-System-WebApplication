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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Persons>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Persons>()
                .Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Persons>()
                .Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Persons>()
                .Property(p => p.Email)
                .HasMaxLength(150);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Persons>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    if (!string.IsNullOrWhiteSpace(entry.Entity.FirstName))
                        entry.Entity.FirstName = Capitalize(entry.Entity.FirstName);

                    if (!string.IsNullOrWhiteSpace(entry.Entity.LastName))
                        entry.Entity.LastName = Capitalize(entry.Entity.LastName);
                }
            }

            return base.SaveChanges();
        }

        private string Capitalize(string value)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(value.ToLower());
        }
    }
}