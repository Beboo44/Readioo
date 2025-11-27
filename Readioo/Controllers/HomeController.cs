using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;
using System.Diagnostics;
using System.Security.Claims;

namespace Readioo.Controllers
{
    [Authorize]  // Protects all actions in this controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenreService _genreService; //  2. Define the service field
        private readonly IBookService _bookService; // 1. Add BookService field
        private readonly IRecommendationService _recommendationService;

        public HomeController(ILogger<HomeController> logger, IBookService bookService, IRecommendationService recommendationService, IGenreService genreService)
        {
            _logger = logger;
            _genreService = genreService;
            _bookService = bookService;
            _recommendationService = recommendationService;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Recommendations
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var recommendations = await _recommendationService.GetRecommendationsForUserAsync(userId);
                ViewBag.Recommendations = recommendations;

                // 2. Genres
                var genres = _genreService.GetAllGenres();
                ViewBag.Genres = genres;

                // 3. Books (Only Recently Added)
                var allBooks = _bookService.GetAllBooks();


                // Recently Added: Newest by Date
                ViewBag.RecentBooks = allBooks.OrderByDescending(b => b.PublishDate).Take(4).ToList();
            }

            

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