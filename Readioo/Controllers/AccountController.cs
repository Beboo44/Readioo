using Microsoft.AspNetCore.Mvc;
using Readioo.ViewModel;

namespace Readioo.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM login)
        {

            return View("~/Views/Home/Index.cshtml",login);
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
