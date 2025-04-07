
namespace Clickly.ServiceContracts
{
    public interface IHashtagsService
    {
        Task ProccessHashtagForNewPostAsync(string postContent);
        Task ProccessHashtagForRemovePostAsync(string postContent);
    }
}
