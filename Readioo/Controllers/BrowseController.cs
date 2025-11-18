using Microsoft.AspNetCore.Mvc;

namespace Readioo.Controllers
{
    public class BrowseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
