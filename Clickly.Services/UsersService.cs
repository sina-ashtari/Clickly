using Clickly.Data;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _dbContext;
        public UsersService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUser(int loggedInUserId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId) ?? new User() { FullName =   ""};
        }

        public async Task<List<Post>> GetUserPosts(int userId)
        {
            var allPosts = await _dbContext.Posts
                .Where(n => n.UserId == userId && n.Reports.Count < 10 && !n.IsDeleted) // the post i created or the post that are public
                .Include(n => n.User)
                .Include(n => n.Like)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User).OrderByDescending(n => n.DateCreated)
                .Include(n => n.Reports)
                .ToListAsync();

            return allPosts;
        }

        public async Task UpdateUserProfilePicture(int userId, string profilePictureUrl)
        {
            var userDatabase = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userDatabase != null) 
            {
                userDatabase.ProfilePictureUrl = profilePictureUrl;
                _dbContext.Users.Update(userDatabase);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
