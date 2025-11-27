using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readioo.Business.Services.Interfaces;

namespace Readioo.Controllers
{
    [Authorize]  // ✅ Protects all Genre actions
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: Genre
        public IActionResult Index()
        {
            var genres = _genreService.GetAllGenres();
            return View(genres);
        }

        // GET: Genre/Details/5
        public IActionResult Details(int id)
        {
            var genreDetails = _genreService.GetGenreById(id);

            if (genreDetails == null)
            {
                return NotFound();
            }

            return View(genreDetails);
        }
    }
}