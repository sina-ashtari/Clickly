using Clickly.Data.Models;

namespace Clickly.ViewModels.Users
{
    public class GetUserProfileVM
    {
        public List<Post> Posts { get; set; }
        public User User { get; set; }
    }
}
