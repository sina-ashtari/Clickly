using Clickly.ServiceContracts;
using Clickly.ViewModels.Friends;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.ViewComponents
{
    public class ConnectionsViewComponent : ViewComponent
    {
        private readonly IFriendsService _friendsService;
        public ConnectionsViewComponent(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {

            var connections = await _friendsService.GetConnectionsAsync(userId);
            var result = new List<UserWithFriendsVM>();
            foreach (var connection in connections) 
            {
                result.Add(new UserWithFriendsVM
                {
                    UserId = connection.User.Id,
                    FullName = connection.User.FullName,
                    ProfilePictureUrl = connection.User.ProfilePictureUrl,
                    FriendsCount = connection.FriendsCount,

                });
            }
            return View(result);
        }
    }
}
