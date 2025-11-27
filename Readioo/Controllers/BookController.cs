using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.Services.Interfaces;
using Readioo.ViewModel;
using System.Security.Claims;

namespace Readioo.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;
        private readonly IShelfService _shelfService;
        private readonly IToastNotification _toast;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IBookService bookService, IAuthorService authorService,
            IShelfService shelfService, IGenreService genreService,
            IToastNotification toast, IWebHostEnvironment webHostEnvironment)
        {
            _bookService = bookService;
            _authorService = authorService;
            _genreService = genreService;
            _shelfService = shelfService;
            _toast = toast;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var books = _bookService.GetAllBooks();
            return View(books);
        }

        public IActionResult Browse()
        {
            // Get all books with their genres included
            var books = _bookService.GetAllBooksWithGenres();
            var genres = _genreService.GetAllGenres();
            ViewBag.Genres = genres;
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var authors = _authorService.getAllAuthors();
            var genres = _genreService.GetAllGenres();

            ViewBag.Genres = genres;
            ViewBag.AuthorList = new SelectList(authors, "AuthorId", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookVM book)
        {
            if (!ModelState.IsValid)
            {
                var authors = _authorService.getAllAuthors();
                var genres = _genreService.GetAllGenres();
                ViewBag.AuthorList = new SelectList(authors, "AuthorId", "FullName");
                ViewBag.Genres = genres;
                return View(book);
            }

            string? uniqueFileName = null;
            if (book.BookImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/books");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString() + "_" + book.BookImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await book.BookImage.CopyToAsync(stream);
                }
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
                BookGenres = book.BookGenres ?? new List<string>(),
                BookImage = uniqueFileName != null ? "images/books/" + uniqueFileName : null
            };

            await _bookService.CreateBook(bookCreatedDto);
            _toast.AddSuccessToastMessage("Book Added Successfully");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var bookDto = _bookService.bookById(id);
            if (bookDto == null) return NotFound();

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
            {
                var userShelves = await _shelfService.GetUserShelves(userId);
                ViewBag.UserShelves = userShelves.Where(s => s.ShelfName != "Favorites").ToList();
            }

            return View(bookDto);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _bookService.bookById(id);
            if (book == null) return NotFound();

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
                BookGenres = book.BookGenres
            };

            var authors = _authorService.getAllAuthors();
            ViewBag.AuthorList = new SelectList(authors, "AuthorId", "FullName", book.AuthorId);

            return View(bookVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int? id, BookVM book)
        {
            if (id is null) return BadRequest();

            string? uniqueFileName = null;
            if (book.BookImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/books");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString() + "_" + book.BookImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await book.BookImage.CopyToAsync(stream);
                }
            }

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
                Description = book.Description
            };

            await _bookService.UpdateBook(bookDto);
            _toast.AddSuccessToastMessage("Book Updated Successfully");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            await _bookService.DeleteBook(id.Value);
            _toast.AddSuccessToastMessage("Book Deleted Successfully");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MyBooks()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userShelves = await _shelfService.GetUserShelves(userId);
            var shelvesWithBooks = new List<Readioo.Business.DataTransferObjects.Shelves.ShelfWithBooksDto>();

            foreach (var shelf in userShelves)
            {
                var books = _shelfService.GetShelfBooks(shelf.ShelfId);
                shelvesWithBooks.Add(new Readioo.Business.DataTransferObjects.Shelves.ShelfWithBooksDto
                {
                    ShelfId = shelf.ShelfId,
                    ShelfName = shelf.ShelfName,
                    Books = books
                });
            }

            var vm = new MyBooksViewModel
            {
                UserId = userId,
                UserName = User.Identity?.Name ?? "User",
                Shelveswithbook = shelvesWithBooks
            };

            return View(vm);
        }

        // ✅ UPDATED: Now uses 'MoveBookToShelfAsync' logic + returns JSON for AJAX
        [HttpPost]
        public async Task<IActionResult> AddToShelf(int? bookId, string? shelfName)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Json(new { success = false, message = "Please login first." });
            }

            if (bookId is null || shelfName is null)
                return Json(new { success = false, message = "Invalid request." });

            try
            {
                // This Service Method (created earlier) handles the logic:
                // "If already on a shelf -> Move it. If not -> Add it."
                var result = await _shelfService.MoveBookToShelfAsync(userId, bookId.Value, shelfName);

                if (result)
                {
                    return Json(new { success = true, message = $"Book added to {shelfName} successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to add book (Shelf not found)." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}