using Demo.DataAccess.Repositories.UoW;
using Microsoft.EntityFrameworkCore;
using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DataTransferObjects.Review;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.Business.DataTransferObjects.Genre;

namespace Readioo.Business.Services.Classes
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ... (bookById, CreateBook, GetAllBooks, DeleteBook methods remain the same) ...
        // For brevity, I am focusing on the modified UpdateBook method below.
        // Ensure you keep the other methods as they were in your previous working version.

        public BookDto? bookById(int id)
        {
            var book = _unitOfWork.BookRepository.GetBookWithDetails(id);
            if (book == null) return null;

            return new BookDto
            {
                BookId = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                Language = book.Language,
                AuthorId = book.AuthorId,
                PagesCount = book.PagesCount,
                PublishDate = book.PublishDate,
                MainCharacters = book.MainCharacters,
                Rate = book.Rate,
                Description = book.Description,
                BookImage = book.BookImage,
                AuthorName = book.Author?.FullName ?? "Unknown",
                BookGenres = book.BookGenres.Select(g => g.Genre.GenreName).ToList(),
                Genres = book.BookGenres.Select(bg => new GenreDto { Id = bg.GenreId, GenreName = bg.Genre.GenreName }).ToList(),
                Reviews = book.Reviews.Select(r => new ReviewDto { ReviewId = r.Id, UserId = r.UserId, BookId = r.BookId, Username = r.User != null ? r.User.FirstName + " " + r.User.LastName : "Unknown", Rating = r.Rating, ReviewText = r.ReviewText, CreatedAt = r.CreatedAt }).ToList()
            };
        }

        public async Task CreateBook(BookCreatedDto bookCreatedDto)
        {
            var newBook = new Book
            {
                Title = bookCreatedDto.Title,
                Isbn = bookCreatedDto.Isbn,
                Language = bookCreatedDto.Language,
                AuthorId = bookCreatedDto.AuthorId,
                PagesCount = bookCreatedDto.PagesCount,
                PublishDate = bookCreatedDto.PublishDate,
                MainCharacters = bookCreatedDto.MainCharacters,
                Description = bookCreatedDto.Description,
                Rate = 0m ,// default rate
            };

            if (bookCreatedDto.BookImage != null)
            {
                newBook.BookImage = bookCreatedDto.BookImage;
            }

            _unitOfWork.BookRepository.Add(newBook);
            await _unitOfWork.CommitAsync();

            if (bookCreatedDto.BookGenres != null && bookCreatedDto.BookGenres.Any())
            {
                var allGenres = _unitOfWork.GenreRepository.GetAll().ToList();
                foreach (var genreName in bookCreatedDto.BookGenres)
                {
                    var genre = allGenres.FirstOrDefault(g =>
                        g.GenreName.Equals(genreName.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (genre != null)
                    {
                        newBook.BookGenres.Add(new BookGenre { BookId = newBook.Id, GenreId = genre.Id });
                    }
                }
                _unitOfWork.BookRepository.Update(newBook);
                await _unitOfWork.CommitAsync();
            }
        }

        public IEnumerable<BookDto> GetAllBooks()
        {
            return _unitOfWork.BookRepository.GetAll()
                .Select(a => new BookDto
                {
                    BookId = a.Id,
                    Title = a.Title,
                    Isbn = a.Isbn,
                    Language = a.Language,
                    AuthorId = a.AuthorId,
                    PagesCount = a.PagesCount,
                    PublishDate = a.PublishDate,
                    MainCharacters = a.MainCharacters,
                    Rate = a.Rate,
                    Description = a.Description,
                    BookImage = a.BookImage,
                    AuthorName = a.Author?.FullName,

                    BookGenres = a.BookGenres?
                    .Select(g => g.Genre.GenreName)
                    .ToList() ?? new List<string>(),

                    Genres = a.BookGenres?
                    .Select(bg => new GenreDto { Id = bg.Genre.Id, GenreName = bg.Genre.GenreName })
                    .ToList()??new List<GenreDto>(),

                    Reviews = a.Reviews?
                    .Select(r => new ReviewDto
                    {
                        ReviewId = r.Id,
                        UserId = r.UserId,
                        Username = (r.User?.FirstName + " " + r.User?.LastName)??"",
                        Rating = r.Rating,
                        ReviewText = r.ReviewText,
                        CreatedAt = r.CreatedAt

                    })
                    .ToList()??new List<ReviewDto>(),

                    BookShelves = a.BookShelves?
                    .Select(s => _unitOfWork.ShelfRepository.GetById(s.ShelfId)?.ShelfName??"")
                    .ToList()?? new List<String>()
                });
        }

        // ✅ MODIFIED METHOD
        public async Task UpdateBook(BookDto bookDto)
        {
            // 1. We need the book WITH details (genres) to modify the collection
            var book = _unitOfWork.BookRepository.GetBookWithDetails(bookDto.BookId);

            if (book == null) throw new Exception("Book Not Found");

            // 2. Update Scalars
            book.Title = bookDto.Title;
            book.Isbn = bookDto.Isbn;
            book.MainCharacters = bookDto.MainCharacters;
            book.Language = bookDto.Language;
            book.AuthorId = bookDto.AuthorId;
            book.PagesCount = bookDto.PagesCount;
            book.PublishDate = bookDto.PublishDate;
            book.Description = bookDto.Description;

            if (bookDto.BookImage != null)
            {
                book.BookImage = bookDto.BookImage;
            }

            // 3. Update Genres
            // First, clear the existing relationships
            book.BookGenres.Clear();

            // Then add the new ones selected by the user
            if (bookDto.BookGenres != null && bookDto.BookGenres.Any())
            {
                var allGenres = _unitOfWork.GenreRepository.GetAll().ToList();

                foreach (var genreName in bookDto.BookGenres)
                {
                    var genre = allGenres.FirstOrDefault(g =>
                        g.GenreName.Equals(genreName.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (genre != null)
                    {
                        book.BookGenres.Add(new BookGenre
                        {
                            BookId = book.Id,
                            GenreId = genre.Id
                        });
                    }
                }
            }

            _unitOfWork.BookRepository.Update(book);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteBook(int id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null) throw new Exception("Book Not Found");
            _unitOfWork.BookRepository.Remove(book);
            await _unitOfWork.CommitAsync();
        }
        public async Task<List<BookDto>> GetUserBooksAsync(int userId)
        {
            // 1️⃣ Get all shelves that belong to the user
            var shelves = _unitOfWork.ShelfRepository
                .GetAll()
                .Where(s => s.UserId == userId)
                .ToList();

            if (!shelves.Any())
                return new List<BookDto>();

            // 2️⃣ Collect all shelf IDs
            var shelfIds = shelves.Select(s => s.Id).ToList();

            // 3️⃣ Get BookShelf entries for these shelves
            var bookShelves = await _unitOfWork.BookShelfRepository
                .Query()                                 // must expose IQueryable
                .Where(bs => shelfIds.Contains(bs.ShelfId))
                .Include(bs => bs.Book)
                    .ThenInclude(b => b.BookGenres)
                        .ThenInclude(bg => bg.Genre)
                .ToListAsync();

            // 4️⃣ Extract unique books
            var books = bookShelves
                .Select(bs => bs.Book)
                .Distinct()
                .ToList();

            // 5️⃣ Map to BookDto
            return books.Select(a => new BookDto
            {
                BookId = a.Id,
                Title = a.Title,
                Isbn = a.Isbn,
                Language = a.Language,
                AuthorId = a.AuthorId,
                PagesCount = a.PagesCount,
                PublishDate = a.PublishDate,
                MainCharacters = a.MainCharacters,
                Rate = a.Rate,
                Description = a.Description,
                BookImage = a.BookImage,
                AuthorName = a.Author?.FullName,

                BookGenres = a.BookGenres
                    .Select(g => g.Genre.GenreName)
                    .ToList()

            }).ToList();
        }


        public IEnumerable<BookDto> GetRecentlyAddedBooks(int count)
        {
            return _unitOfWork.BookRepository.GetAll().OrderByDescending(b => b.Id).Take(count).Select(b => new BookDto { BookId = b.Id, Title = b.Title, BookImage = b.BookImage, Rate = b.Rate, AuthorName = b.Author != null ? b.Author.FullName : "Unknown" }).ToList();
        }
    }
}