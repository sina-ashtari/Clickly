using Clickly.Data;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Services.Services
{
    public class ChatsService : IChatsService
    {
        private readonly ApplicationDbContext _dbContext;
        public ChatsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Message>> GetMessagesAsync(int currentUserId, int targetUserId)
        {
            var messages = await _dbContext.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == targetUserId) || (m.SenderId == targetUserId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
            return messages;
        }

        public async Task<List<Message>> GetPrivateChatsAsync(int userId)
        {
            var messages = await _dbContext.Messages
        .Where(m => m.SenderId == userId || m.ReceiverId == userId)
        .OrderByDescending(m => m.SentAt)
        .Include(m => m.Sender)
        .Include(m => m.Receiver)
        .ToListAsync();

            var latestMessages = messages
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => g.First())
                .ToList();

            return latestMessages;
        }
    }
}
