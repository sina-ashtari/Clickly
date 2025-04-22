using Clickly.Controllers.Base;
using Clickly.Data.Helper.Constants;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Friends;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class FriendsController : BaseController
    {
        private readonly IFriendsService _friendsService;
        private readonly INotificationsService _notificationsService;
        public FriendsController(IFriendsService friendsService, INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
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
            var userName = GetFullName();

            await _friendsService.SendRequestAsync(userId.Value, receiverId);
            await _notificationsService.AddNotificationAsync(userId: receiverId, userFullName: userName, notificationType: NotificationType.FriendRequest, postId: null);
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
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();
            var userName = GetFullName();

            var request = await _friendsService.UpdateRequestStatusAsync(requestId, status);
            if(status == FriendshipStatus.Accepted) await _notificationsService.AddNotificationAsync(userId: request.SenderId, userFullName: userName, notificationType: NotificationType.FriendRequestApproved, postId: null);
            return RedirectToAction("Index");

        }
    }
}