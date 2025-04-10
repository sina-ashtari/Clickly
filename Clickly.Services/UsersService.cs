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
