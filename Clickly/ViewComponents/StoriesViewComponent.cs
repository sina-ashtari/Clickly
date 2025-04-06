using Clickly.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clickly.ViewComponents
{
    public class StoriesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _dbContext;
        public StoriesViewComponent(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories = await _dbContext.Stories.Where(n => n.DateCreated >= DateTime.UtcNow.AddHours(-24)).Include(s => s.User).ToListAsync();
            return View(allStories);
        }
    }
}
