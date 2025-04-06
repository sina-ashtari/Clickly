using Clickly.Data;
using Clickly.Data.Helper;
using Clickly.Data.Models;
using Clickly.ViewModels.Home;
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
            int loggedInUser = 1;
            // we are left joining users with posts 
            var allPosts = await _dbContext.Posts
                .Where(n => (!n.IsPrivate || n.UserId == loggedInUser) && n.Reports.Count < 10 && !n.IsDeleted) // the post i created or the post that are public
                .Include(n => n.User)
                .Include(n => n.Like)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User ).OrderByDescending(n => n.DateCreated)
                .Include(n => n.Reports)
                .ToListAsync();

            return View(allPosts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
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
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                // validating if the provided image type is correct then saving it in wwwroot folder
                if (post.Image.ContentType.Contains("image"))
                {
                    // first we trying to create root folder in that case we dont have it
                    string rootFolderPathImage = Path.Combine(rootFolderPath, "images/posts");
                    Directory.CreateDirectory(rootFolderPathImage);

                    // then creating file name with defined Guid for each of them
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(post.Image.FileName); 
                    string filePath = Path.Combine(rootFolderPathImage, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await post.Image.CopyToAsync(stream);

                        newPost.Image = "/images/posts/" + fileName;
                    }

                }

            }

            // adding post
            await _dbContext.Posts.AddAsync(newPost);  
            await _dbContext.SaveChangesAsync();

            // find and store hashtags 
            var postHashtags = HashtagHelper.GetHashtags(post.Content);
            if (postHashtags != null) 
            {
                foreach (var hashtag in postHashtags) 
                {
                    var hashtagDb = await _dbContext.Hashtags.FirstOrDefaultAsync(n => n.Name == hashtag);

                    if (hashtagDb != null)
                    {
                        hashtagDb.Count += 1;
                        hashtagDb.DateUpdated = DateTime.UtcNow;

                        _dbContext.Hashtags.Update(hashtagDb);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        var newHashtag = new Hashtags { Name = hashtag, Count = 1, DateCreated = DateTime.UtcNow, DateUpdated = DateTime.UtcNow };
                        await _dbContext.Hashtags.AddAsync(newHashtag);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            // redirecting to index page
            return RedirectToAction("Index");

        }

        [HttpPost]
        public  async Task<IActionResult> TogglePostLike(PostLikeVM postLike)
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
        [HttpPost]
        public async Task<IActionResult> AddPostComment(PostCommentVM postComment)
        {
            int loggedInUser = 1;
            var newComment = new Comment() 
            {
                PostId = postComment.PostId,
                Content = postComment.Content,
                UserId = loggedInUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            await _dbContext.Comments.AddAsync(newComment);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");   
        }

        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM comment)
        {
            var commentDb = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == comment.CommentId);
            if (commentDb != null)
            {
                _dbContext.Comments.Remove(commentDb);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavorite)
        {
            int loggedInUser = 1;
            // checking if the user already liked the post or not.  
            var favorite = await _dbContext.Favorites.Where(like => like.PostId == postFavorite.PostId && like.UserId == loggedInUser).FirstOrDefaultAsync();
            if (favorite != null)
            {
                _dbContext.Favorites.Remove(favorite);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var newfavorite = new Favorite()
                {
                    PostId = postFavorite.PostId,
                    UserId = loggedInUser,
                };
                await _dbContext.Favorites.AddAsync(newfavorite);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibility)
        {
            int loggedInUser = 1;
            // checking if the user already liked the post or not.  
            var post = await _dbContext.Posts.FirstOrDefaultAsync(l => l.Id == postVisibility.PostId && l.UserId == loggedInUser);
            if (post != null)
            {
                post.IsPrivate = !post.IsPrivate;
                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReport)
        {
            int loggedInUser = 1;
            var newReport = new Report()
            {
                UserId = loggedInUser,
                PostId = postReport.PostID,
                DateCreated = DateTime.UtcNow,
                
            };

            await _dbContext.Reports.AddAsync(newReport);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
            
        public async Task<IActionResult> PostDelete(PostDeleteVM postDelete)
        {

            var postDatabase = await _dbContext.Posts.FirstOrDefaultAsync(c => c.Id == postDelete.PostId);
            if (postDatabase != null)
            {
                postDatabase.IsDeleted = true;
                _dbContext.Posts.Remove(postDatabase);
                await _dbContext.SaveChangesAsync();
            }
            if(postDatabase != null)
            {
                var postHashtag = HashtagHelper.GetHashtags(postDatabase.Content);
                foreach (var hashtag in postHashtag)
                {
                    var hashtagDb = await _dbContext.Hashtags.FirstOrDefaultAsync(n => n.Name == hashtag);
                    if (hashtagDb != null) 
                    {
                        hashtagDb.Count -= 1;
                        hashtagDb.DateUpdated = DateTime.UtcNow;

                        _dbContext.Hashtags.Update(hashtagDb);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            
            return RedirectToAction("index");
        }

    }
}
