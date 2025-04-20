using Clickly.Data;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Services.Services
{
    public class StoriesService : IStoriesService
    {
        private readonly ApplicationDbContext _dbContext;
        public StoriesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Story> CreateStoryAsync(Story story)
        {
            
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
