using System.Security.Claims;
using Clickly.Data;
using Clickly.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Clickly.Services.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;
        public ChatHub(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
        }
        public async Task SendMessage(string toUserId, string message, string? uploadedFileUrl)
        {
            var fromUserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(fromUserId))
                throw new Exception("Sender user ID is null.");

            try
            {
                var msg = new Message
                {
                    SenderId = int.Parse(fromUserId),
                    ReceiverId = int.Parse(toUserId),
                    Text = message,
                    SentAt = DateTime.UtcNow,
                    UploadedFileUrl = uploadedFileUrl
                };

                _dbContext.Messages.Add(msg);
                await _dbContext.SaveChangesAsync();

                await Clients.User(toUserId).SendAsync("ReceiveMessage", fromUserId, message, uploadedFileUrl);
                await Clients.Caller.SendAsync("ReceiveMessage", fromUserId, message, uploadedFileUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SignalR] SendMessage failed: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
