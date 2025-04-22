using Clickly.Data;
using Clickly.Data.Helper;
using Clickly.Data.Models;
using Clickly.Hubs;
using Clickly.ServiceContracts;
using Clickly.Services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clickly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // database string connection goes here.
            var dbConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(dbConnection);
            });

            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IHashtagsService, HashtagsService>();
            builder.Services.AddScoped<IStoriesService, StoriesService>();
            builder.Services.AddScoped<IFilesService, FilesService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IFriendsService, FriendsService>();
            builder.Services.AddScoped<INotificationsService, NotificationsService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            // Identity configuration
            builder.Services.AddIdentity<User, IdentityRole<int>>( options =>
            {
                // the redirection not completedd 
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.AccessDeniedPath = "/Authentication/AccessDenied";
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Auth:Google:ClientID"] ?? "";
                options.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"] ?? "";
                options.CallbackPath = "/signin-google";
            }).AddGitHub(options =>
            {
                options.ClientId = builder.Configuration["Auth:Github:ClientID"] ?? "";
                options.ClientSecret = builder.Configuration["Auth:Github:ClientSecret"] ?? "";
                options.CallbackPath = "/signin-github";
            });

            builder.Services.AddAuthorization();
            builder.Services.AddSignalR();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // seeding database with initializers data 
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync(); // make us sure that migration will be executed.
                await DbInitializer.SeedAsync(dbContext);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                await DbInitializer.SeedUsersAndRolesAsync(userManager, roleManager);
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization(); 
            

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHub<NotificationHub>("/notificationHub");
            app.Run();
        }
    }
}

