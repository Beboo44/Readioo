using Microsoft.AspNetCore.Mvc;
using Readioo.Business.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Readioo.Controllers
{
    public class ShelfController : Controller
    {
        private readonly IShelfService _shelfService;

        public ShelfController(IShelfService shelfService)
        {
            _shelfService = shelfService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Adds security check for the token we added in the View
        public async Task<IActionResult> MoveBook(int bookId, string shelfName)
        {
            // 1. Get the current User ID
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            try
            {
                // 2. Call the Service to move the book
                var result = await _shelfService.MoveBookToShelfAsync(userId, bookId, shelfName);

                if (result)
                {
                    return Json(new { success = true, message = "Book moved successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Shelf not found or move failed." });
                }
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}