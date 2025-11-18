using Microsoft.AspNetCore.Mvc;

namespace Readioo.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Show()
        {
            return View();
        }
    }
}
