using Microsoft.AspNetCore.Mvc;

namespace Readioo.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
