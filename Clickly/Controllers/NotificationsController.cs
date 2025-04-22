using Clickly.Controllers.Base;
using Clickly.Data.Helper.Constants;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class NotificationsController : BaseController
    {
        private readonly INotificationsService _notificationsService;
        public NotificationsController(INotificationsService notificationsService) 
        { 
            _notificationsService = notificationsService;
        }
        
        public async Task<IActionResult> GetNotifications()
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();
            var notifications = await _notificationsService.GetNotifications(userId.Value);
            return PartialView("Notifications/_Notifications", notifications);
        }
        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var count = await _notificationsService.GetUnreadNotificationsCount(userId.Value);
            return Json(count);
        }
        [HttpPost]
        public async Task<IActionResult> SetNotificationAsRead(int notificationId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            await _notificationsService.SetNotificationAsReadAsync(notificationId);
            var notifications = await _notificationsService.GetNotifications(userId.Value);
            return PartialView("Notifications/_Notifications", notifications);

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
