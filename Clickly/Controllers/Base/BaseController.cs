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
        protected string? GetFullName()
        {
            var loggedInUserFullName = User.FindFirstValue(ClaimTypes.Name);
            //var GivenName = User.FindFirstValue(ClaimTypes.GivenName);
            //var Surname = User.FindFirstValue(ClaimTypes.Surname);

            return loggedInUserFullName;
        }
        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Authentication");
        }
    }
}
