using Clickly.Data.Models;

namespace Clickly.Data.Dtos
{
    public class UserWithFriendsDto
    {
        public User User { get; set; }  
        public int FriendsCount { get; set; }
    }
}
