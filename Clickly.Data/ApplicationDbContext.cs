using Clickly.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Hashtags> Hashtags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // We have many post, and each post have one user, and that one user have user id related to post with user id.
            modelBuilder.Entity<User>()
                .HasMany(user => user.Posts)
                .WithOne(post => post.User)
                .HasForeignKey(post => post.UserId);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Stories)
                .WithOne(post => post.User)
                .HasForeignKey(post => post.UserId);

            modelBuilder.Entity<Like>()
                .HasKey(like => new { like.PostId, like.UserId });

            modelBuilder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Like)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
            .HasOne(l => l.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.PostId, f.UserId });  

            modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Post)
            .WithMany(p => p.Favorites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasKey(f => new { f.PostId, f.UserId });

            modelBuilder.Entity<Report>()
            .HasOne(f => f.Post)
            .WithMany(p => p.Reports)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Report>()
            .HasOne(f => f.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);


            // Customize identity model tables name
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        }
    }
}
