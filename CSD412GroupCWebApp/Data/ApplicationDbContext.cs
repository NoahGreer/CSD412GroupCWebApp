using System;
using System.Collections.Generic;
using System.Text;
using CSD412GroupCWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSD412GroupCWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(new List<Category>()
            {
                new Category() { Id = 1, Name = "Programming", Description = "Articles about programming" },
                new Category() { Id = 2, Name = "Music", Description = "Articles about music" },
                new Category() { Id = 3, Name = "Movies", Description = "Articles about movies" },
                new Category() { Id = 4, Name = "Art", Description = "Articles about art" },
                new Category() { Id = 5, Name = "Travel", Description = "Articles about travel" },
                new Category() { Id = 6, Name = "Science", Description = "Articles about science" },
            });
        }

        public DbSet<CSD412GroupCWebApp.Models.Category> Category { get; set; }

        public DbSet<CSD412GroupCWebApp.Models.Blog> Blog { get; set; }

        public DbSet<CSD412GroupCWebApp.Models.Article> Article { get; set; }
    }
}
