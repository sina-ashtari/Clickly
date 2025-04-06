using Clickly.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clickly.ViewComponents
{
    public class HashtagsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _dbContext;
        public HashtagsViewComponent(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
            var topThreeHashtags = await _dbContext.Hashtags
                .Where(h => h.DateCreated >= oneWeekAgo)
                .OrderByDescending(n => n.Count)
                .Take(3)
                .ToListAsync();
            return View(topThreeHashtags);  
        }
    }
}
