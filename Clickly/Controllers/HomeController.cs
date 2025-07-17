using Clickly.Controllers.Base;
using Clickly.Data.Helper.Constants;
using Clickly.Data.Helper.Enums;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        
        private readonly IHashtagsService _hashtagsService;
        private readonly IPostService _postService;
        private readonly IFilesService _filesService;
        private readonly INotificationsService _notificationsService;

        public HomeController(IHashtagsService hashtagsService, IPostService postService, IFilesService filesService,  INotificationsService notificationsService)
        {
            _hashtagsService = hashtagsService;
           
            _postService = postService;
            _filesService = filesService;   
            _notificationsService = notificationsService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUser = GetUserId();
            if (loggedInUser == null) return RedirectToLogin();

            // we are left joining users with posts 
            var allPosts = await _postService.GetAllPostsAsync(loggedInUser.Value);

            return View(allPosts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            var loggedInUser = GetUserId();
            if (loggedInUser == null) return RedirectToLogin();

            var imageUploadPath = await _filesService.UploadImageAsync(post.Image, ImageFileType.PostImage);
            var newPost = new Post() { 
            Content = post.Content,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow,
            Image = imageUploadPath,
            NumberOfReport = 0,
            UserId = loggedInUser.Value
            };
            if(newPost.Content == null) return RedirectToAction("Index");
            await _postService.CreatePostAsync(newPost);

            // find and store hashtags 
            await _hashtagsService.ProccessHashtagForNewPostAsync(post.Content);
            // redirecting to index page
            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> TogglePostLike(PostLikeVM postLike)
        {
            var loggedInUser = GetUserId();
            var userName = GetFullName();
            if (loggedInUser == null) return RedirectToLogin();
            // checking if the user already liked the post or not.  
            var result = await _postService.TogglePostLikeAsync(postLike.PostId, loggedInUser.Value);
            

            var post = await _postService.GetPostByIdAsync(postLike.PostId);
            if (result.SendNotification && loggedInUser != post.UserId) await _notificationsService.AddNotificationAsync(userId: post.UserId, notificationType: NotificationType.Like, postId: postLike.PostId, userFullName: userName);
            return PartialView("Home/_Post", post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPostComment(PostCommentVM postComment)
        {
            var loggedInUser = GetUserId();
            var userName = GetFullName();
            if (loggedInUser == null) return RedirectToLogin();

            var newComment = new Comment() 
            {
                PostId = postComment.PostId,
                Content = postComment.Content,
                UserId = loggedInUser.Value,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            await _postService.AddPostCommentAsync(newComment);
            var post = await _postService.GetPostByIdAsync(postComment.PostId);
            if (loggedInUser != post.UserId)
            await _notificationsService.AddNotificationAsync(userId: post.UserId,notificationType: NotificationType.Comment,postId: postComment.PostId, userFullName : userName);
            return PartialView("Home/_post", post);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM comment)
        {
            await _postService.DeletePostCommentAsync(comment.CommentId);
            var post = await _postService.GetPostByIdAsync(comment.PostId);
            return PartialView("Home/_Post", post);
            
        }


        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavorite)
        {
            var loggedInUser = GetUserId();
            var userName = GetFullName();
            if (loggedInUser == null) return RedirectToLogin();
            var result = await _postService.TogglePostFavoriteAsync(postFavorite.PostId, loggedInUser.Value);

            var post = await _postService.GetPostByIdAsync(postFavorite.PostId);
            if (result.SendNotification && loggedInUser != post.UserId) await _notificationsService.AddNotificationAsync(userId : post.UserId,notificationType: NotificationType.Favorite, postId : postFavorite.PostId,userFullName: userName);
            return PartialView("Home/_Post", post);
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibility)
        {
            var loggedInUser = GetUserId();
            if (loggedInUser == null) return RedirectToLogin();

            await _postService.ToggleVisibilityPostAsync(postVisibility.PostId, loggedInUser.Value);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReport)
        {
            var loggedInUser = GetUserId();
            if (loggedInUser == null) return RedirectToLogin();
            await _postService.ReportPostAsync(postReport.PostID, loggedInUser.Value);
            return RedirectToAction("Index");
        }
            
        public async Task<IActionResult> PostDelete(PostDeleteVM postDelete)
        {

            var postRemoved = await _postService.RemovePostAsync(postDelete.PostId);

            await _hashtagsService.ProccessHashtagForRemovePostAsync(postRemoved.Content);
            
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Details(int postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            return View(post);
        }


    }
}
