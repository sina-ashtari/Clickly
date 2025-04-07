using Clickly.Data;
using Clickly.Data.Helper;
using Clickly.ServiceContracts;
using Clickly.Services;
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

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // seeding database with initializers data 
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync(); // make us sure that migration will be executed.
                await DbInitializer.SeedAsync(dbContext);
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}

