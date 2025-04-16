namespace Clickly.ViewModels.Friends
{
    public class UserWithFriendsVM
    {
        public int UserId { get; set; }
        public string FullName{ get; set; }
        public string ProfilePictureUrl { get; set; }
        public int FriendsCount { get; set; }
        public string FriendCountDisplay => FriendsCount == 0 ? "No followers" : FriendsCount == 1 ? "One follower" : $"{FriendsCount} followers";
    }
}
