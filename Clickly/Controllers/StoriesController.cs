using Clickly.Data;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Stories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Controllers
{
    public class StoriesController : Controller
    {
        private readonly IStoriesService _storiesService;
        public StoriesController(IStoriesService storiesService)
        {
            _storiesService = storiesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM story)
            {
            int loggedInUser = 1;
            var newStory = new Story()
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                UserId = loggedInUser
            };
           
            await _storiesService.CreateStoryAsync(newStory, story.Image);
            return RedirectToAction("Index", "Home");
        }
    }
}
