using Clickly.Data;
using Clickly.Data.Models;
using Clickly.ViewModels.Stories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Controllers
{
    public class StoriesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public StoriesController(ApplicationDbContext context)
        {
            _dbContext = context;
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
            if (story.Image != null && story.Image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                // validating if the provided image type is correct then saving it in wwwroot folder
                if (story.Image.ContentType.Contains("image"))
                {
                    // first we trying to create root folder in that case we dont have it
                    string rootFolderPathImage = Path.Combine(rootFolderPath, "images/stories");
                    Directory.CreateDirectory(rootFolderPathImage);

                    // then creating file name with defined Guid for each of them
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(story.Image.FileName);
                    string filePath = Path.Combine(rootFolderPathImage, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await story .Image.CopyToAsync(stream);

                        newStory.Image = "/images/stories/" + fileName;
                    }

                }

            }
            await _dbContext.Stories.AddAsync(newStory);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
