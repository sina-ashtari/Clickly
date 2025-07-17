using Clickly.Controllers.Base;
using Clickly.Data.Helper.Constants;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class UsersController : BaseController
    {

        private readonly IUsersService _usersService;
        private readonly UserManager<User> _userManager;

        public UsersController(IUsersService usersService, UserManager<User> userManager)
        {
            _usersService = usersService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int userId) 
        {
            var currentUserId = GetUserId();
            if (currentUserId == null) RedirectToLogin();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userPost = await _usersService.GetUserPosts(userId);

            var userProfileVM = new GetUserProfileVM()
            {
                CurrentUserId = currentUserId.Value,
                Posts = userPost,
                User = user
            };
            return View(userProfileVM);
        }
    }
}
