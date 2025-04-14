using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected int ? GetUserId()
        {
            var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUser))
                return null;
            return int.Parse(loggedInUser);
        }
        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Authentication");
        }
    }
}
