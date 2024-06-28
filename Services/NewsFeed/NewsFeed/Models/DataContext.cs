using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<HashtagPost> HashtagPost { get; set; }

        public DataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.PostComments)
                .WithOne(pc => pc.Author)
                .HasForeignKey(pc => pc.AuthorId);

            modelBuilder.Entity<Post>()
                .HasMany(pc => pc.PostComments)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);
            
            modelBuilder.Entity<Post>()
                .HasMany(ph => ph.PostHashtags)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);

            modelBuilder.Entity<Hashtag>()
                .HasMany(p => p.HashtagPosts)
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
