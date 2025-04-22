using System.Security.Claims;
using Clickly.Controllers.Base;
using Clickly.Data.Helper.Constants;
using Clickly.Data.Helper.Enums;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    [Authorize(Roles = $"{AppRoles.User},{AppRoles.Admin}")]
    public class SettingsController : BaseController
    {
        private readonly IUsersService _usersService;
        private readonly IFilesService _filesService;
        private readonly UserManager<User> _userManager;
        public SettingsController(IUsersService usersService, IFilesService filesService, UserManager<User> userManager)
        {
            _usersService = usersService;
            _filesService = filesService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            var loggedInUser = await _userManager.GetUserAsync(User);

            return View(loggedInUser);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(UpdateProfilePictureVM profilePictureVM)
        {
            var loggedInUser = GetUserId();
            if (loggedInUser == null) return RedirectToLogin();

            var uploadedProfilePictureUrl = await _filesService.UploadImageAsync(profilePictureVM.ProfilePictureImage, ImageFileType.ProfileImage);

            await _usersService.UpdateUserProfilePicture(loggedInUser.Value, uploadedProfilePictureUrl);
            return RedirectToAction("Index");
        }
        
        

    }
}
