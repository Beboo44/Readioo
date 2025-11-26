using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readioo.Business.DataTransferObjects.Review;
using Readioo.Business.Services.Interfaces;
using System.Security.Claims;

namespace Readioo.Controllers
{
    [Authorize]  // ✅ Protects all Book actions

    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(ReviewDto dto)
        {
            // 1. Validate inputs
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Book", new { id = dto.BookId });
            }

            // 2. Get logged-in user ID (DO NOT TRUST THE CLIENT)
            dto.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 3. Add review
            await _reviewService.AddReviewAsync(dto);

            // 4. Redirect back to book page
            return RedirectToAction("Details", "Book", new { id = dto.BookId });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
