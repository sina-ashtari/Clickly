using Clickly.Data.Models;

namespace Clickly.ServiceContracts
{
    public interface IChatsService
    {
        Task<List<Message>> GetMessagesAsync(int currentUserId, int targetUserId);
        Task<List<Message>> GetPrivateChatsAsync(int userId);
    }
}
