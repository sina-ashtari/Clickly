using Clickly.Data;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Services.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbContext;
        public AdminService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ApproveReportAsync(int postId)
        {
            var postDatabase = await _dbContext.Posts.FirstOrDefaultAsync(n => n.Id == postId);
            if (postDatabase != null) 
            {
                postDatabase.IsDeleted = true;
                _dbContext.Posts.Update(postDatabase);
                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<List<Post>> GetReportedPostAsync()
        {
            //var post = await _dbContext.Posts.Include(n => n.Reports).ThenInclude(n => n.User) // for case that users can be reported too
            //    .Where(n => n.Reports.Count >= 5 && !n.IsDeleted)
            //    .ToListAsync();

            var post = await _dbContext.Posts
                .Include(n => n.User)
                .Where(n => n.NumberOfReport > 5 & !n.IsDeleted).ToListAsync();
            return post;

        }

        public async Task RejectReportAsync(int postId)
        {
            var postDatabase = await _dbContext.Posts.FirstOrDefaultAsync(n => n.Id == postId);
            if (postDatabase != null)
            {
                postDatabase.NumberOfReport = 0;
                _dbContext.Posts.Update(postDatabase);
            }
            var postReports = await _dbContext.Reports.Where(n => n.PostId == postId).ToListAsync();
            if (postReports.Any())
            {
                _dbContext.Reports.RemoveRange(postReports);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
