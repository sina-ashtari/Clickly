using Clickly.Data;
using Clickly.Data.Helper;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHashtagsService _hashtagsService;
        private readonly IPostService _postService;

        public HomeController(ILogger<HomeController> logger, IHashtagsService hashtagsService, IPostService postService)
        {
            _hashtagsService = hashtagsService;
            _logger = logger;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            int loggedInUser = 1;
            // we are left joining users with posts 
            var allPosts = await _postService.GetAllPostsAsync(loggedInUser);

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

            await _postService.CreatePostAsync(newPost, post.Image);

            // find and store hashtags 
            await _hashtagsService.ProccessHashtagForNewPostAsync(post.Content);
            // redirecting to index page
            return RedirectToAction("Index");

        }

        [HttpPost]
        public  async Task<IActionResult> TogglePostLike(PostLikeVM postLike)
        {
            int loggedInUser = 1;
            // checking if the user already liked the post or not.  
            await _postService.TogglePostLikeAsync(postLike.PostId, loggedInUser);
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
            await _postService.AddPostCommentAsync(newComment);
            return RedirectToAction("Index");   
        }

        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM comment)
        {
            await _postService.DeletePostCommentAsync(comment.CommentId);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavorite)
        {
            int loggedInUser = 1;
            await _postService.TogglePostFavoriteAsync(postFavorite.PostId, loggedInUser);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibility)
        {
            int loggedInUser = 1;
            
            await _postService.ToggleVisibilityPostAsync(postVisibility.PostId, loggedInUser);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReport)
        {
            int loggedInUser = 1;
            await _postService.ReportPostAsync(postReport.PostID, loggedInUser);
            return RedirectToAction("Index");
        }
            
        public async Task<IActionResult> PostDelete(PostDeleteVM postDelete)
        {

            var postRemoved = await _postService.RemovePostAsync(postDelete.PostId);

            await _hashtagsService.ProccessHashtagForRemovePostAsync(postRemoved.Content);
            
            return RedirectToAction("index");
        }

    }
}
