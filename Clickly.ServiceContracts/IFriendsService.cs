
using Clickly.Data.Dtos;
using Clickly.Data.Models;

namespace Clickly.ServiceContracts
{
    public interface IFriendsService
    {
        Task<bool> SendRequestAsync(int senderId, int receiverId);
        Task<FriendRequest> UpdateRequestStatusAsync(int requestId, string status);
        
        Task<List<UserWithFriendsDto>> GetSuggestedFriendsAsync(int userId);
        Task<List<FriendRequest>> GetSentFriendRequestAsync(int userId);
        Task<List<FriendRequest>> GetReceivedFriendRequestAsync(int userId);
        Task<List<Friendship>> GetFriendsAsync(int userId);
        Task RemoveFriendAsync(int friendshipId);
        Task<List<UserWithFriendsDto>> GetConnectionsAsync(int userId);
    }
}
