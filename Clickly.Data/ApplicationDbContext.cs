using Clickly.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // We have many post, and each post have one user, and that one user have user id related to post with user id.
            modelBuilder.Entity<User>().HasMany(user => user.Posts).WithOne(post => post.User).HasForeignKey(post => post.UserId);
            modelBuilder.Entity<Like>().HasKey(like => new { like.PostId, like.UserId }); // ? 
            modelBuilder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Like)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
