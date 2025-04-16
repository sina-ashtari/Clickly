using Clickly.Data.Models;

namespace Clickly.ViewModels.Friends
{
    public class FriendshipVM
    {
        public List<FriendRequest> FriendRequestsSent = new List<FriendRequest>();
        public List<FriendRequest> FriendRequestReceived = new List<FriendRequest>();
        public List<Friendship> Friends = new List<Friendship>();
    }
}
