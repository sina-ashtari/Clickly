
using Clickly.Data.Models;

namespace Clickly.ServiceContracts
{
    public interface IUsersService
    {
        Task<User> GetUser(int loggedInUserId);
        Task UpdateUserProfilePicture(int userId, string profilePictureUrl);
    }
}
