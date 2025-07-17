using Clickly.Data.Models;

namespace Clickly.ViewModels.Chats
{
    public class PrivateChatVM
    {
        public int CurrentUserId { get; set; }
        public int TargetUserId { get; set; }
        public string? TargetFullName { get; set; }
        public string? CurrentFullName { get; set; }
        public string? TargetProfilePicture { get; set; }
        public string? CurrentProfilePicture { get; set; }
        public List<Message>? Messages { get; set; }
    }
}
