using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Hashtag> Hashtag { get; set; }
        public DbSet<NewsComment> NewsComment { get; set; }
        public DbSet<HashtagNews> HashtagNews { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(emp => emp.NewsList)
                .WithOne(news => news.Author)
                .HasForeignKey(news => news.AuthorId);

            modelBuilder.Entity<Employee>()
                .HasMany(emp => emp.NewsCommentList)
                .WithOne(nc => nc.Author)
                .HasForeignKey(nc => nc.AuthorId);

            modelBuilder.Entity<News>()
                .HasMany(news => news.NewsCommentList)
                .WithOne(nc => nc.News)
                .HasForeignKey(nc => nc.NewsId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<News>()
                .HasMany(news => news.HashtagNewsList)
                .WithOne(hn => hn.News)
                .HasForeignKey(hn => hn.NewsId);

            modelBuilder.Entity<Hashtag>()
                .HasMany(h => h.HashtagNewsList)
                .WithOne(hn => hn.Hashtag)
                .HasForeignKey(hn => hn.HashtagId);

            modelBuilder.Entity<News>()
                .Property(news => news.Likes)
                .HasDefaultValue(0);

            modelBuilder.Entity<News>()
                .Property(news => news.CreatedAt)
                .HasDefaultValue(DateTime.Now);
            
            modelBuilder.Entity<News>()
                .Property(news => news.UpdatedAt)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<NewsComment>()
                .Property(nc => nc.CreatedAt)
                .HasDefaultValue(DateTime.Now);
            
            modelBuilder.Entity<NewsComment>()
                .Property(nc => nc.UpdatedAt)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<NewsComment>()
                .Property(nc => nc.Id)
                .HasDefaultValue(Guid.NewGuid());

            modelBuilder.Entity<News>()
                .Property(n => n.Id)
                .HasDefaultValue(Guid.NewGuid());

            modelBuilder.Entity<Hashtag>()
                .Property(h => h.Id)
                .HasDefaultValue(Guid.NewGuid());

            modelBuilder.Entity<HashtagNews>()
                .Property(hn => hn.Id)
                .HasDefaultValue(Guid.NewGuid());

            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .HasDefaultValue(Guid.NewGuid());

            base.OnModelCreating(modelBuilder);
        }
    }
}
