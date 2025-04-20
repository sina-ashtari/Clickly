using Clickly.Data;
using Clickly.Data.Dtos;
using Clickly.Data.Helper.Constants;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Services.Services
{
    public class FriendsService : IFriendsService
    {

        private readonly ApplicationDbContext _dbContext;
        public FriendsService(ApplicationDbContext  dbContext)
        {
             _dbContext = dbContext;
        }

        public async Task<List<Friendship>> GetFriendsAsync(int userId)
        {
            return await _dbContext.Friendships
                .Include(n => n.Sender) 
                .Include(n => n.Receiver)
                .Where(n => n.SenderId == userId || n.ReceiverId == userId)
                .ToListAsync();

        }

        public async Task<List<FriendRequest>> GetReceivedFriendRequestAsync(int userId)
        {
            var friendRequestSent = await _dbContext.FriendRequests
                .Include(n => n.Sender) // it is optional
                .Include(n => n.Receiver)
                .Where(f => f.ReceiverId == userId && f.Status == FriendshipStatus.Pending)
                .ToListAsync();

            return friendRequestSent;
        }

        public async Task<List<FriendRequest>> GetSentFriendRequestAsync(int userId)
        {
            var friendRequestSent = await _dbContext.FriendRequests
                .Include(n => n.Sender) // it is optional
                .Include(n => n.Receiver)
                .Where(f => f.SenderId == userId && f.Status == FriendshipStatus.Pending)
                .ToListAsync();

            return friendRequestSent;
        }

        public async Task<List<UserWithFriendsDto>> GetSuggestedFriendsAsync(int userId)
        {
            var existingFriendsIds = await _dbContext.Friendships
                .Where(n => n.SenderId == userId || n.ReceiverId == userId)
                .Select(n => n.SenderId == userId ? n.ReceiverId : n.SenderId)
                .ToListAsync();

            // pending requests
            var pendingRequestIds = await _dbContext.FriendRequests
                .Where(n => (n.SenderId == userId || n.ReceiverId == userId) && n.Status == FriendshipStatus.Pending)
                .Select(n => n.SenderId == userId ? n.ReceiverId : n.SenderId)
                .ToListAsync();

            // getting suggested friend
            var suggestedFriends = await _dbContext.Users
                .Where(n => n.Id != userId && !existingFriendsIds.Contains(n.Id) && !pendingRequestIds.Contains(n.Id))
                .Select(user => new UserWithFriendsDto()
                {
                    User = user,
                    FriendsCount = _dbContext.Friendships.Count(friend => friend.SenderId == user.Id || friend.ReceiverId == user.Id),

                })
                .Take(5)
                .ToListAsync();

            return suggestedFriends;
        }

        public async Task RemoveFriendAsync(int frienshipId)
        {
            var friendship = await _dbContext.Friendships.FirstOrDefaultAsync(n => n.Id == frienshipId);
            if (friendship != null)
            {
                _dbContext.Friendships.Remove(friendship);
                await _dbContext.SaveChangesAsync();

                //find requests
                var requests = await _dbContext.FriendRequests
                    .Where(r => r.SenderId == friendship.SenderId && r.ReceiverId == friendship.ReceiverId ||
                    r.SenderId == friendship.ReceiverId && r.ReceiverId == friendship.SenderId)
                    .ToListAsync();

                if (requests.Any())
                {
                    _dbContext.FriendRequests.RemoveRange(requests);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> SendRequestAsync(int senderId, int receiverId)
        {

            var request = new FriendRequest()
            {

                SenderId = senderId,
                ReceiverId = receiverId,
                Status = FriendshipStatus.Pending,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            _dbContext.FriendRequests.Add(request);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<FriendRequest> UpdateRequestStatusAsync(int requestId, string status)
        {
            var requestDb = await _dbContext.FriendRequests.FirstOrDefaultAsync(n => n.Id == requestId);

            if (requestDb != null)
            {
                requestDb.Status = status;
                requestDb.DateUpdated = DateTime.UtcNow;
                _dbContext.FriendRequests.Update(requestDb);
                if(status == FriendshipStatus.Accepted)
                {
                    var friendShipConnection = new Friendship()
                    {
                        SenderId = requestDb.SenderId,
                        ReceiverId = requestDb.ReceiverId,
                        DateCreated = DateTime.UtcNow
                    };
                    _dbContext.Friendships.Update(friendShipConnection);
                }
                await _dbContext.SaveChangesAsync();
            }
            return requestDb;
        }
    }
}
