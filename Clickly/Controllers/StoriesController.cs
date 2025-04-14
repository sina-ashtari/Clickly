using Clickly.Controllers.Base;
using Clickly.Data.Helper.Enums;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Stories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    [Authorize]
    public class StoriesController : BaseController
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
            var loggedInUser = GetUserId();
            if (loggedInUser == null) return RedirectToLogin();

            var imageUploadPath = await _filesService.UploadImageAsync(story.Image, ImageFileType.StoryImage);
            var newStory = new Story()
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                Image = imageUploadPath,
                UserId = loggedInUser.Value
            };
           
            await _storiesService.CreateStoryAsync(newStory);
            return RedirectToAction("Index", "Home");
        }
    }
}
