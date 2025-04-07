using Clickly.Data.Models;

namespace Clickly.ServiceContracts
{
    public interface IStoriesService
    {
        Task<List<Story>> GetAllStoriesAsync();
        Task<Story> CreateStoryAsync(Story story);
    }
}
