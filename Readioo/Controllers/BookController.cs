using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;
using System.Drawing;

namespace Readioo.Controllers
{
    public class BookController : Controller
    {

        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        public BookController(IBookService bookService, IAuthorService authorService)
        {
            _bookService = bookService;
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create(string searchAuthor = "")
        {
            var authors = _authorService.getAllAuthors();

            if (!string.IsNullOrEmpty(searchAuthor))
            {
                authors = authors.Where(a => a.FullName.Contains(searchAuthor, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Pass authors to ViewBag for the dropdown
            ViewBag.AuthorList = new SelectList(authors, "AuthorId", "FullName");

            return View();
        }

        [HttpPost]
        public IActionResult Create(BookVM book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            BookCreatedDto bookCreatedDto = new BookCreatedDto()
            {
                Title = book.Title,
                Isbn = book.Isbn,
                Language = book.Language,
                AuthorId = book.AuthorId,
                PagesCount = book.PagesCount,
                MainCharacters = book.MainCharacters,
                PublishDate = book.PublishDate,
            };
            if (book.BookImage != null)
            {
                string SaveFolder = "wwwroot/images/books/";
                SaveFolder += Guid.NewGuid().ToString() + "_" + book.BookImage.FileName;
                book.BookImage.CopyTo(new FileStream(SaveFolder, FileMode.Create));

                bookCreatedDto.BookImage = SaveFolder; 
            }

            _bookService.CreateBook(bookCreatedDto);
            return RedirectToAction(nameof(Index));
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
