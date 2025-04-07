
using Clickly.Data;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Services
{
    public class StoriesService : IStoriesService
    {
        private readonly ApplicationDbContext _dbContext;
        public StoriesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Story> CreateStoryAsync(Story story, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                // validating if the provided image type is correct then saving it in wwwroot folder
                if (image.ContentType.Contains("image"))
                {
                    // first we trying to create root folder in that case we dont have it
                    string rootFolderPathImage = Path.Combine(rootFolderPath, "images/stories");
                    Directory.CreateDirectory(rootFolderPathImage);

                    // then creating file name with defined Guid for each of them
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(rootFolderPathImage, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);

                        story.Image = "/images/stories/" + fileName;
                    }
                }
            }
            await _dbContext.Stories.AddAsync(story);
            await _dbContext.SaveChangesAsync();
            return story;
        }

        public async Task<List<Story>> GetAllStoriesAsync()
        {
            var allStories = await _dbContext.Stories.Where(n => n.DateCreated >= DateTime.UtcNow.AddHours(-24)).Include(s => s.User).ToListAsync();
            return allStories;
        }
    }
}
