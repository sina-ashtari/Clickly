using Clickly.Data.Helper.Constants;
using Clickly.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Clickly.Data.Helper
{
    public static class DbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            // roles
            if (!roleManager.Roles.Any()) 
            {
                foreach (var roleName in AppRoles.All) 
                {
                    if (!await roleManager.RoleExistsAsync(roleName)) 
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                    }
                }
            }

            // users with roles
            if (!userManager.Users.Any(n => !string.IsNullOrEmpty(n.Email))) 
            {
                var userPassword = "password";
                var newUser = new User { 
                    FullName = "Sina Ashtari",
                    Email = "email@email.com", 
                    UserName = "sina",
                    ProfilePictureUrl = "",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newUser, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, AppRoles.User);
                }

                var newAdmin = new User
                {
                    FullName = "admin",
                    Email = "admin@email.com",
                    ProfilePictureUrl = "",
                    EmailConfirmed = true
                };
                var resultNewAdmin = await userManager.CreateAsync(newAdmin, userPassword);
                if (resultNewAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, AppRoles.Admin);
                }
            }

        }
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
