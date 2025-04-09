using Clickly.Data.Helper.Enums;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IFilesService _filesService;
        public SettingsController(IUsersService usersService, IFilesService filesService)
        {
            _usersService = usersService;
            _filesService = filesService;
        }
        public async Task<IActionResult> Index()
        {
            var loggedInUserId = 1;
            var userDatabase = await _usersService.GetUser(loggedInUserId);
            return View(userDatabase);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(UpdateProfilePictureVM profilePictureVM)
        {
            int loggedInUserId = 1;
            var uploadedProfilePictureUrl = await _filesService.UploadImageAsync(profilePictureVM.ProfilePictureImage, ImageFileType.ProfileImage);

            await _usersService.UpdateUserProfilePicture(loggedInUserId, uploadedProfilePictureUrl);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileVM profileVM)
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM updatePasswordVM)
        {
            return RedirectToAction("Index");
        }

    }
}
