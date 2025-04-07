
using Clickly.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Clickly.ServiceContracts
{
    public interface IStoriesService
    {
        Task<List<Story>> GetAllStoriesAsync();
        Task<Story> CreateStoryAsync(Story story, IFormFile image);
    }
}
