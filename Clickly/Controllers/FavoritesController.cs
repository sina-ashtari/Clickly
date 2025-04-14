using System.Security.Claims;
using Clickly.Controllers.Base;
using Clickly.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    [Authorize]
    public class FavoritesController : BaseController
    {
        private readonly IPostService _postService;
        public FavoritesController(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<IActionResult> Index()
        {
            
            var loggedInUser = GetUserId();
            if (loggedInUser == null) return RedirectToLogin();
            var myFavoritePost = await _postService.GetAllFavoritedPostAsync(loggedInUser.Value);
            return View(myFavoritePost);
        }
    }
}
