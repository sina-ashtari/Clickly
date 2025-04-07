using Clickly.Data.Helper.Enums;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Stories;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    public class StoriesController : Controller
    {
        private readonly IStoriesService _storiesService;
        private readonly IFilesService _filesService;
        public StoriesController(IStoriesService storiesService, IFilesService filesService)
        {
            _storiesService = storiesService;
            _filesService = filesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM story)
            {
            int loggedInUser = 1;
            var imageUploadPath = await _filesService.UploadImageAsync(story.Image, ImageFileType.StoryImage);
            var newStory = new Story()
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                Image = imageUploadPath,
                UserId = loggedInUser
            };
           
            await _storiesService.CreateStoryAsync(newStory);
            return RedirectToAction("Index", "Home");
        }
    }
}
