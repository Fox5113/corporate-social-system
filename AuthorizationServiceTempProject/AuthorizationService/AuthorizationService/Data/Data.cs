using AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<HashtagPost> HashtagPosts { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<PostComment> PostComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<HashtagPost>()
                .HasOne<Post>()
                .WithMany()
                .HasForeignKey(hp => hp.PostId);

            modelBuilder.Entity<HashtagPost>()
                .HasOne<Hashtag>()
                .WithMany()
                .HasForeignKey(hp => hp.HashtagId);

            modelBuilder.Entity<Models.File>()
                .HasOne<Post>()
                .WithMany()
                .HasForeignKey(f => f.PostId);

            modelBuilder.Entity<PostComment>()
                .HasOne<Post>()
                .WithMany()
                .HasForeignKey(pc => pc.PostId);

            modelBuilder.Entity<PostComment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(pc => pc.UserId);
        }
    }
}