using Microsoft.AspNetCore.Mvc;
using Readioo.Business.Services.Classes;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;
using System.Diagnostics;
using System.Security.Claims;

namespace Readioo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;
        private readonly IRecommendationService _recommendationService;

        public HomeController(ILogger<HomeController> logger, IBookService bookService, IRecommendationService recommendationService)
        {
            _logger = logger;
            _bookService = bookService;
            _recommendationService = recommendationService;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Get Recommendations (if logged in)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var recommendations = await _recommendationService.GetRecommendationsForUserAsync(userId);
                ViewBag.Recommendations = recommendations;
            }

            // 2. Fetch All Books
            var allBooks = _bookService.GetAllBooks();

            // 3. Pass Data via ViewBag
            ViewBag.TrendingBooks = allBooks.OrderByDescending(b => b.Rate).Take(6).ToList();
            ViewBag.RecentBooks = allBooks.OrderByDescending(b => b.PublishDate).Take(6).ToList();

            // 4. RETURN THE CORRECT MODEL
            // FIX: Do NOT return 'allBooks'. Return 'new LoginVM()' because your View expects @model LoginVM
            return View(new LoginVM());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
