using Microsoft.EntityFrameworkCore;
using VirtualSports.BE.Models.DatabaseModels;

namespace VirtualSports.BE.Contexts
{
    public class DatabaseManagerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DatabaseManagerContext(DbContextOptions<DatabaseManagerContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users");
            modelBuilder.Entity<Game>()
                .ToTable("Games");
            modelBuilder.Entity<Category>()
                .ToTable("Categories");
            modelBuilder.Entity<Provider>()
                .ToTable("Providers");
            modelBuilder.Entity<Tag>()
                .ToTable("Tags");
        }
    }
}
