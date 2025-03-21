using Clickly.Data.Models;

namespace Clickly.Data.Helper
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // in case that we dont have any posts or any users, so we creating a new user with post as initializer.
            if (!context.Users.Any() && !context.Posts.Any())
            {
                var newUser = new User()
                {
                    FullName = "Sina Ashtari",
                    ProfilePictureUrl = ""
                };
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();

                var newPostWithoutImage = new Post()
                {
                    UserId = newUser.Id,
                    Content = "This is first post of Clickly!",
                    Image = "",
                    NumberOfReport = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                };

                var newPostWithImage = new Post()
                {
                    UserId = newUser.Id,
                    Content = "This is first post (with image!) of Clickly!",
                    Image = "",
                    NumberOfReport = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                };

                await context.Posts.AddRangeAsync(newPostWithoutImage, newPostWithImage);
                await context.SaveChangesAsync();
            }
        }
    }
}
