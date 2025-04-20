using Clickly.Data;
using Clickly.Data.Helper.Constants;
using Clickly.Data.Models;
using Clickly.Hubs;
using Clickly.ServiceContracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Services.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly ApplicationDbContext _dbContext; 
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationsService(ApplicationDbContext dbContext, IHubContext<NotificationHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }
        public async Task AddNotificationAsync(int userId, string userFullName, int? postId, string notificationType)
        {
            var newNotification = new Notification()
            {
                UserId = userId,
                Message = GetPostMessage(notificationType, userFullName),
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Type = notificationType,
                IsRead = false,
                PostId = postId.HasValue ? postId.Value : null,
            };
            var notificationCount = await GetUnreadNotificationsCount(userId);
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notificationCount);

            await _dbContext.Notifications.AddAsync(newNotification);
            await _dbContext.SaveChangesAsync();    
            
        }

        public async Task<List<Notification>> GetNotifications(int userId)
        {
            var allNotifications = await _dbContext.Notifications.Where(n => n.UserId ==  userId)
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.DateCreated)
                .ToListAsync();
            return allNotifications;
        }

        public async Task<int> GetUnreadNotificationsCount(int userId)
        {
            var count = await _dbContext.Notifications.Where(n => n.UserId == userId && !n.IsRead).CountAsync();
            return count;
        }

        public async Task SetNotificationAsReadAsync(int notificationId)
        {
            var notificationDatabase = await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
            if (notificationDatabase != null) 
            {
                notificationDatabase.DateUpdated = DateTime.UtcNow;
                notificationDatabase.IsRead = true;

                _dbContext.Notifications.Update(notificationDatabase);
                await _dbContext.SaveChangesAsync();
            }
        }

        private string GetPostMessage(string notificationType, string userFullName)
        {
            var message = "";
            switch (notificationType)
            {
                case NotificationType.Like :
                    message = $"{userFullName} liked your post.";
                    break;
                case NotificationType.Comment:
                    message = $"{userFullName} added a comment to your post.";
                    break;
                case NotificationType.Favorite:
                    message = $"{userFullName} favorited your post.";
                    break;
                case NotificationType.FriendRequest:
                    message = $"{userFullName} wants to create a connection.";
                    break;
                case NotificationType.FriendRequestApproved:
                    message = $"{userFullName} approved your connection.";
                    break;
                default: 
                    message = "";
                    break;
            }
            return message;
        }
    }
}
