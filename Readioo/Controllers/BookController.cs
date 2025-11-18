using Microsoft.AspNetCore.Mvc;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;

namespace Readioo.Controllers
{
    public class BookController : Controller
    {

        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(BookVM book)
        {
            return View(book);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var book = _bookService.bookById(id);
            if(book is null)
            {
                return NotFound();
            }
            return View(book);
        }
    }
}
