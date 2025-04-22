using System.Security.Claims;
using Clickly.Controllers.Base;
using Clickly.Data.Helper.Constants;
using Clickly.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    [Authorize(Roles = AppRoles.User)]
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
