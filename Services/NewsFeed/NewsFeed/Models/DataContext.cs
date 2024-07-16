using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<NewsComment> NewsComments { get; set; }
        public DbSet<HashtagNews> HashtagNews { get; set; }

        public DataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(u => u.NewsList)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<Employee>()
                .HasMany(u => u.NewsComments)
                .WithOne(pc => pc.Author)
                .HasForeignKey(pc => pc.AuthorId);

            modelBuilder.Entity<News>()
                .HasMany(pc => pc.NewsComments)
                .WithOne(p => p.News)
                .HasForeignKey(p => p.NewsId);
            
            modelBuilder.Entity<News>()
                .HasMany(ph => ph.NewsHashtags)
                .WithOne(p => p.News)
                .HasForeignKey(p => p.NewsId);

            modelBuilder.Entity<Hashtag>()
                .HasMany(p => p.HashtagNews)
                .WithOne(p => p.Hashtag)
                .HasForeignKey(p => p.HashtagId);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Host=localhost;Username=admin;Password=password;Database=NewsFeed");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
