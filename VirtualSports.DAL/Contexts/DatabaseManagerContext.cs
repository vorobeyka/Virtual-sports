using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualSports.DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VirtualSports.DAL.Contexts
{
    /// <inheritdoc />
    public class DatabaseManagerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ExpSession> ExpSessions { get; set; }

        /// <inheritdoc />
        public DatabaseManagerContext(DbContextOptions<DatabaseManagerContext> options) : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }

        /// <inheritdoc />
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
            modelBuilder.Entity<ExpSession>()
                .ToTable("ExpiredSessions");
        }
    }
}
