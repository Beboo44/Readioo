using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Readioo.Business.Services.Classes;
using Readioo.Business.Services.Interfaces;

namespace Readioo.Controllers
{
    public class AdminController : Controller
    {
        
        private readonly IUserService _userService;
        private readonly IToastNotification _toast;


        public AdminController( IUserService userService, IToastNotification toast)
        {
            
            _userService = userService;
            _toast = toast;
            
        }
        public async Task <IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                _toast.AddWarningToastMessage("Page Not Found");
                return RedirectToAction("Login", "Account");
            }
            var user = await _userService.GetUserByIdAsync(int.Parse(userIdString));

            if(user== null || !user.IsAdmin)
            {
                _toast.AddWarningToastMessage("Page Not Found");
                return RedirectToAction("Index", "Home");

            }


            return View();
        }
    }
}
