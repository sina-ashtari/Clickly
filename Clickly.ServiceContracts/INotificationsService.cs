using Clickly.Data.Models;

namespace Clickly.ServiceContracts
{
    public interface INotificationsService
    {
        Task AddNotificationAsync(int userId, string userFullName, int? postId, string notificationType);
        Task<int> GetUnreadNotificationsCount(int userId);
        Task<List<Notification>> GetNotifications(int userId);
        Task SetNotificationAsReadAsync(int notificationId);
    }
}
