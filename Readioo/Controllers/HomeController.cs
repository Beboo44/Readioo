using Microsoft.AspNetCore.Mvc;
using Readioo.Business.Services.Interfaces; //  1. Import the Service Interface
using Readioo.Models;
using Readioo.ViewModel; // Needed if you are initializing the LoginVM in the Index
using System.Diagnostics;

namespace Readioo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenreService _genreService; //  2. Define the service field
        private readonly IBookService _bookService; // 1. Add BookService field

        //  3. Inject the service in the constructor
        public HomeController(ILogger<HomeController> logger, IGenreService genreService, IBookService bookService)
        {
            _logger = logger;
            _genreService = genreService;
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            //  4. Fetch genres and store them in ViewBag
            // This allows the view to loop through them without changing your main LoginVM model
            var genres = _genreService.GetAllGenres();
            ViewBag.Genres = genres;
            // Fetch recently added books and store them in ViewBag
            var recentBooks = _bookService.GetRecentlyAddedBooks(4);
            ViewBag.RecentBooks = recentBooks;

            // Assuming you are passing a LoginVM based on your previous View code
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