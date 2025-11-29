using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.Services.Classes;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;
using System.Drawing.Printing;
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
        private readonly IUserService _userService;
        private readonly IToastNotification _toast;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IBookService bookService, IAuthorService authorService,
            IShelfService shelfService, IGenreService genreService,
            IToastNotification toast, IWebHostEnvironment webHostEnvironment, IUserService userService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _genreService = genreService;
            _shelfService = shelfService;
            _userService = userService;
            _toast = toast;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                _toast.AddWarningToastMessage("Page Not Found");
                return RedirectToAction("Login", "Account");
            }
            var user = await _userService.GetUserByIdAsync(int.Parse(userIdString));

            if (user == null || !user.IsAdmin)
            {
                _toast.AddWarningToastMessage("Page Not Found");
                return RedirectToAction("Index", "Home");

            }
            var books = _bookService.GetAllBooks();
            return View(books);
        }

        public IActionResult Browse(string term = "", int page=1)
        {
            var books = _bookService.GetAllBooks();
            if (!string.IsNullOrWhiteSpace(term))
                books = _bookService.SearchBooks(term);

            var genres = _genreService.GetAllGenres();

            int pageSize = 12;
            int totalBooks = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

            var pagedBooks = books.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();


            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Genres = genres;

            return View(pagedBooks);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                _toast.AddWarningToastMessage("Page Not Found");
                return RedirectToAction("Login", "Account");
            }
            var user = await _userService.GetUserByIdAsync(int.Parse(userIdString));

            if (user == null || !user.IsAdmin)
            {
                _toast.AddWarningToastMessage("Page Not Found");
                return RedirectToAction("Index", "Home");

            }
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
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var book = _bookService.bookById(id);
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (user is null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = int.Parse(user);
            if (userId > 0)
            {
                book.UserRating = await _bookService.GetUserRating(userId, id);
            }

            ViewBag.UserShelves = await _userService.GetUserShelvesAsync(userId);

            if (book is null)
                return NotFound();

            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                _toast.AddWarningToastMessage("Page Not Found");
                return RedirectToAction("Login", "Account");
            }
            var user = await _userService.GetUserByIdAsync(int.Parse(userIdString));

            if (user == null || !user.IsAdmin)
            {
                _toast.AddWarningToastMessage("Page Not Found");
                return RedirectToAction("Index", "Home");

            }
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
                BookImg = book.BookImage,
                BookGenres = book.BookGenres ?? new List<string>()
            };

            var authors = _authorService.getAllAuthors();

            ViewBag.BookId = id;
            ViewBag.AuthorId = new SelectList(authors, "AuthorId", "FullName");

            var genres = _genreService.GetAllGenres();
            ViewBag.Genres = genres;



            return View(bookVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int? id, BookVM book)
        {
            if (id is null) return BadRequest();

            if (!ModelState.IsValid)
            {
                var authors = _authorService.getAllAuthors();
                ViewBag.AuthorId = new SelectList(authors, "AuthorId", "FullName");

                // Reload Genres so checkboxes don't disappear on error
                ViewBag.Genres = _genreService.GetAllGenres();

                return View(book);
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
                Description = book.Description,
                BookGenres = book.BookGenres ?? new List<string>()
            };

            if (book.BookImage != null)
            {
                string saveFolder = "images/books";
                saveFolder += Guid.NewGuid().ToString() + "_" + book.BookImage.FileName;
                string serverPath = Path.Combine("wwwroot", saveFolder);
                book.BookImage.CopyTo(new FileStream(serverPath, FileMode.Create));

                bookDto.BookImage = saveFolder;
            }

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

        [HttpPost]
        public async Task<IActionResult> AddToShelf(int? bookId, string? shelfName)
        {


            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }


            if (bookId is null || shelfName is null)
                _toast.AddWarningToastMessage("Invalid request.");


            try
            {

                var result = await _shelfService.MoveBookToShelfAsync(userId, bookId.Value, shelfName);

                if (result)
                {
                    _toast.AddSuccessToastMessage($"Book added to {shelfName} successfully!");
                }
                else
                {
                    _toast.AddWarningToastMessage("Failed to add book (Shelf not found).");
                }
            }
            catch (Exception ex)
            {
                _toast.AddErrorToastMessage("Error: " + ex.Message);
            }

            return RedirectToAction("Browse", "Book");
        }
        [HttpPost]
        public async Task<IActionResult> RateBook(int bookId, int rating)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId <= 0)
                return Unauthorized();

            await _bookService.SaveUserRating(userId, bookId, rating);

            return RedirectToAction("Details", new { id = bookId });
        }


        [HttpGet]
        public IActionResult SearchBooks(string term)
        {
            var results = _bookService.SearchBooks(term);
            return Json(results);
        }

        [HttpGet]
        public IActionResult Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return View(new List<BookDto>());
            }

            var books = _bookService.SearchBooks(term);
            return View(books); // goes to Views/Book/Search.cshtml
        }

    }
}