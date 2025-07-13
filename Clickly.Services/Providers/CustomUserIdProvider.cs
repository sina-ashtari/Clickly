using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Clickly.Services.Providers
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
}
