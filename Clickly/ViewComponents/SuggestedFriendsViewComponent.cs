using System.Security.Claims;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Friends;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.ViewComponents
{
    public class SuggestedFriendsViewComponent : ViewComponent
    {
        public readonly IFriendsService _friendsService;
        public SuggestedFriendsViewComponent(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loggedInUserId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier); 
            var suggestedFriends = await _friendsService.GetSuggestedFriendsAsync(int.Parse(loggedInUserId));
            var suggestedFriendsVM = suggestedFriends.Select(n => new UserWithFriendsVM()
            {
                UserId = n.User.Id,
                FullName = n.User.FullName,
                ProfilePictureUrl = n.User.ProfilePictureUrl,
                FriendsCount = n.FriendsCount,
            }).ToList();
            return View(suggestedFriendsVM);
        }
    }
}
