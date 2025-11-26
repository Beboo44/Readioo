using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.Services.Classes;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;
using System.Drawing;
using System.Security.Claims;

namespace Readioo.Controllers
{
    public class BookController : Controller
    {

        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IUserService _userService;
        private readonly IGenreService _genreService;
        private readonly IShelfService _shelfService;
        
        public BookController(IBookService bookService, IAuthorService authorService, IUserService userService, IShelfService shelfService, IGenreService genreService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _userService = userService;
            _genreService = genreService;
            _shelfService = shelfService;
        }

        public IActionResult Index()
        {
            var books = _bookService.GetAllBooks();
                
            return View(books);
        }
        public IActionResult Browse()
        {
            var books = _bookService.GetAllBooks();

            return View(books);
        }

        [HttpGet]
        public IActionResult Create(/*string searchAuthor = ""*/)
        {
            var authors = _authorService.getAllAuthors();
            var genres = _genreService.GetAllGenres();

            ViewBag.Genres = genres;
            ViewBag.AuthorList = new SelectList(authors, "AuthorId", "FullName");

            return View();
            //if (!string.IsNullOrEmpty(searchAuthor))
            //{
            //    authors = authors.Where(a => a.FullName.Contains(searchAuthor, StringComparison.OrdinalIgnoreCase)).ToList();
            //}
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookVM book)
        {
            if (!ModelState.IsValid)
            {
                var authors = _authorService.getAllAuthors();
                ViewBag.AuthorList = new SelectList(authors, "AuthorId", "FullName");
                return View(book);
            }

            BookCreatedDto bookCreatedDto = new BookCreatedDto()
            {
                Title = book.Title,
                Isbn = book.Isbn,
                Language = book.Language,
                AuthorId = book.AuthorId,
                PagesCount = book.PagesCount,
                Description = book.Description,
                MainCharacters = book.MainCharacters,
                PublishDate = book.PublishDate,
                BookGenres = book.BookGenres ?? new List<string>()  
            };
    
            if (book.BookImage != null)
            {
                string SaveFolder = "images/books/";
                SaveFolder += Guid.NewGuid().ToString() + "_" + book.BookImage.FileName;

                string SavePath = Path.Combine("wwwroot", SaveFolder);
                book.BookImage.CopyTo(new FileStream(SavePath, FileMode.Create));

                bookCreatedDto.BookImage = SaveFolder; 
            }

            await _bookService.CreateBook(bookCreatedDto);
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

        [HttpGet]

        public IActionResult Edit(int id)
        {
            var book = _bookService.bookById(id);
            if (book is null)
                return NotFound();


            var bookVM = new BookVM
            {
                Title = book.Title,
                Isbn = book.Isbn,
                Language = book.Language,
                AuthorId = book.AuthorId,
                PagesCount = book.PagesCount,
                PublishDate = book.PublishDate,
                MainCharacters = book.MainCharacters,
                Description = book.Description,
                BookImg = book.BookImage
            };
            var authors = _authorService.getAllAuthors();
            ViewBag.AuthorId = new SelectList(authors, "AuthorId", "FullName");

            return View(bookVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int? id,BookVM book)
        {
            if (id is null)
                return BadRequest();  //error 404

            if (ModelState is null)
                return NotFound();

            var bookDto = new BookDto()
            {
                BookId = id.Value,
                Title = book.Title,
                Isbn = book.Isbn,
                MainCharacters = book.MainCharacters,
                Language = book.Language,
                AuthorId = book.AuthorId,
                PagesCount = book.PagesCount,
                PublishDate = book.PublishDate,
                
            };
            if(book.BookImage != null)
            {
                string SaveFolder = "images/books/";
                SaveFolder += Guid.NewGuid().ToString() + "_" + book.BookImage.FileName;

                string SavePath = Path.Combine("wwwroot",SaveFolder);
                book.BookImage.CopyTo(new FileStream(SavePath, FileMode.Create));

                bookDto.BookImage = SaveFolder;
            }
            await _bookService.UpdateBook(bookDto);

            return RedirectToAction(nameof(Index));
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id is null)
                return BadRequest();


            await _bookService.DeleteBook(id.Value);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task <IActionResult> MyBooks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login","Account");
            }

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            var books = _bookService.GetAllBooks();

            return View(books); 
        }

        [HttpPost]
        public async Task<IActionResult>AddToShelf(int? bookId, string? shelfName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            if (bookId is null || shelfName is null)
                return BadRequest();

            var shelves = await _userService.GetUserShelvesAsync(int.Parse(userId));
            var shelf = shelves.FirstOrDefault(a => a.ShelfName == shelfName);
            var favoriteShelf = shelves.FirstOrDefault(a => a.ShelfName == "Favorites");

            var book = _bookService.bookById(bookId.Value);

            await _shelfService.AddBook(bookId.Value, shelf.ShelfId,favoriteShelf.ShelfId);


            return RedirectToAction("Browse", "Book");
        }
    }
}
