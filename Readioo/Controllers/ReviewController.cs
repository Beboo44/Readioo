using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readioo.Business.DataTransferObjects.Review;
using Readioo.Business.Services.Interfaces;
using System.Security.Claims;

namespace Readioo.Controllers
{
    [Authorize]  

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
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Book", new { id = dto.BookId });
            }

            dto.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _reviewService.AddReviewAsync(dto);

            return RedirectToAction("Details", "Book", new { id = dto.BookId });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
