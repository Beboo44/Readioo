using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Readioo.Controllers
{
    [Authorize]  // ✅ Protects all Book actions

    public class BrowseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
