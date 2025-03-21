using System.Diagnostics;
using Clickly.Data;
using Clickly.Data.Models;
using Clickly.Models;
using Clickly.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            // we are left joining users with posts 
            var allPosts = _dbContext.Posts.Include(n => n.User).OrderByDescending(n => n.DateCreated).ToListAsync();
            return View(allPosts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel post)
        {
            int loggedIn = 1; // we will change it to authentication later
            var newPost = new Post() { 
            Content = post.Content,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow,
            Image = "",
            NumberOfReport = 0,
            UserId = loggedIn

            };

            // in case that user uploaded the image
            if (post.Image != null && post.Image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwrooot");
                // validating if the provided image type is correct then saving it in wwwroot folder
                if (post.Image.ContentType.Contains("image"))
                {
                    // first we trying to create root folder in that case we dont have it
                    string rootFolderPathImage = Path.Combine(rootFolderPath, "image/uploaded");
                    Directory.CreateDirectory(rootFolderPathImage);

                    // then creating file name with defined Guid for each of them
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(post.Image.FileName); 
                    string filePath = Path.Combine(rootFolderPathImage, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await post.Image.CopyToAsync(stream);

                        newPost.Image = "/image/uploaded/" + fileName;
                    }

                }

            }

            // adding post
            await _dbContext.Posts.AddAsync(newPost);  
            await _dbContext.SaveChangesAsync();
            // redirecting to index page
            return RedirectToAction("Index");

        }

        [HttpPost]
        public  async Task<IActionResult> TogglePostLike(PostLikeViewModel postLike)
        {
            int loggedInUser = 1;
            // checking if the user already liked the post or not.  
            var like = await _dbContext.Likes.Where(like => like.PostId == postLike.PostId && like.UserId == loggedInUser).FirstOrDefaultAsync();
            if (like != null) 
            {
                _dbContext.Likes.Remove(like);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var newLike = new Like() 
                {
                    PostId = postLike.PostId,
                    UserId = loggedInUser,
                };
                await _dbContext.Likes.AddAsync(newLike);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
