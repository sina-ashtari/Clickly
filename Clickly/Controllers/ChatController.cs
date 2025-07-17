using Clickly.Controllers.Base;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Clickly.ViewModels.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers
{
    
    public class ChatController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IChatsService _chatsService;
        public ChatController(UserManager<User> userManager, IChatsService chatsService)
        {
            _userManager = userManager;
            _chatsService = chatsService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var loggedInUser = GetUserId();
            if (loggedInUser == null) return RedirectToLogin();

            var messages = await _chatsService.GetPrivateChatsAsync(loggedInUser.Value);

            var chatPreviews = messages.Select(message =>
            {
                var partner = message.SenderId == loggedInUser ? message.Receiver : message.Sender;

                return new ChatPreviewVM
                {
                    Id = message.Id,
                    PartnerId = partner.Id,
                    PartnerName = partner.FullName,
                    PartnerProfilePicture = partner.ProfilePictureUrl,
                    LastMessage = !string.IsNullOrEmpty(message.Text) ? message.Text : "[Attachment]",
                    SentAt = message.SentAt
                };
            }).ToList();

            return View(chatPreviews);
        }

        [Authorize]
        public async Task<IActionResult> Private(int id)
        {
            var loggedInUser = GetUserId();
            if (loggedInUser == null) return RedirectToLogin();

            var currentUser = await _userManager.GetUserAsync(User);
            var targetUser = await _userManager.FindByIdAsync(id.ToString());

            if (currentUser is null || targetUser is null || targetUser == currentUser) { return NotFound(); }
            var model = new PrivateChatVM
            {
                CurrentUserId = currentUser.Id,
                TargetUserId = targetUser.Id,
                TargetFullName = targetUser.FullName,
                CurrentFullName = currentUser.FullName,
                TargetProfilePicture = "~" + targetUser.ProfilePictureUrl,
                CurrentProfilePicture = "~" + currentUser.ProfilePictureUrl,
                Messages = await _chatsService.GetMessagesAsync(currentUser.Id, id)
                
            };
            

            return View(model);
        }

        [Authorize]
        [HttpPost("/Chat/Private/Upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var userId = GetUserId();
            if(userId == null) return RedirectToLogin();

            if (file == null || file.Length == 0) return BadRequest("No file attached or uploaded.");
            
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/chat/{userId}");
            Directory.CreateDirectory(uploadsPath);
            var filePath = Path.Combine(uploadsPath, fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"/images/chat/{userId}/{fileName}";
            return Ok(new { url });
        }
    }
}
