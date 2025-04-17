using Clickly.Controllers.Base;
using Clickly.Data.Helper.Constants;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Friends;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{

    public class FriendsController : BaseController
    {
        private readonly IFriendsService _friendsService;
        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }
        public async Task<IActionResult> Index()
        {

            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var friendsData = new FriendshipVM()
            {
                FriendRequestsSent = await _friendsService.GetSentFriendRequestAsync(userId.Value),
                FriendRequestReceived = await _friendsService.GetReceivedFriendRequestAsync(userId.Value),
                Friends = await _friendsService.GetFriendsAsync(userId.Value),
            };
            return View(friendsData);
        }
        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(int receiverId) 
        { 
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            await _friendsService.SendRequestAsync(userId.Value, receiverId);
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public async Task<IActionResult> RemoveFriend(int friendshipId)
        {
            await _friendsService.RemoveFriendAsync(friendshipId);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> UpdateFriendRequest(int requestId, string status)
        {
            await _friendsService.UpdateRequestStatusAsync(requestId, status);
            return RedirectToAction("Index");

        }
    }
}