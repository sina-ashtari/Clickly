using Clickly.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IPostService _postService;
        public FavoritesController(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<IActionResult> Index()
        {
            int loggedInUserId = 1;
            var myFavoritePost = await _postService.GetAllFavoritedPostAsync(loggedInUserId);
            return View(myFavoritePost);
        }
    }
}
