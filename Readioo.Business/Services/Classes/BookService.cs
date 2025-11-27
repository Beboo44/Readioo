using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DataTransferObjects.Review;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Classes
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
                AuthorName = book.Author?.FullName,

                // Map DB Entities -> String List for viewing
                BookGenres = book.BookGenres
                    .Select(g => g.Genre.GenreName)
                    .ToList(),
                    // ⭐ Add Reviews to DTO
                Reviews = book.Reviews.Select(r => new ReviewDto
                {
                    ReviewId = r.Id,
                    UserId = r.UserId,
                    BookId = r.BookId,
                    Username = r.User != null
                        ? r.User.FirstName + " " + r.User.LastName
                        : "Unknown",
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    CreatedAt = r.CreatedAt
                }).ToList()
            };
        }

        public async Task CreateBook(BookCreatedDto bookCreatedDto)
        {
            // Create the Book Object
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
                        newBook.BookGenres.Add(new BookGenre
                        {
                            GenreId = genre.Id
                        });
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
                    AuthorName = a.Author.FullName,

                    BookGenres = a.BookGenres
                    .Select(g => g.Genre.GenreName)
                    .ToList(),

                    Reviews = a.Reviews
                    .Select(r => new ReviewDto
                    {
                        ReviewId = r.Id,
                        UserId = r.UserId,
                        Username = r.User.FirstName + " " + r.User.LastName,
                        Rating = r.Rating,
                        ReviewText = r.ReviewText,
                        CreatedAt = r.CreatedAt

                    })
                    .ToList(),

                    BookShelves = a.BookShelves
                    .Select(s => _unitOfWork.ShelfRepository.GetById(s.ShelfId).ShelfName)
                    .ToList()
                });
        }

        public async Task UpdateBook(BookDto bookDto)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(bookDto.BookId);
            if (book == null) throw new Exception("Book Not Found");

            book.Title = bookDto.Title;
            book.Isbn = bookDto.Isbn;
            book.MainCharacters = bookDto.MainCharacters;
            book.Language = bookDto.Language;
            book.AuthorId = bookDto.AuthorId;
            book.PagesCount = bookDto.PagesCount;
            book.PublishDate = bookDto.PublishDate;
            if (bookDto.BookImage != null) book.BookImage = bookDto.BookImage;

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

        public IEnumerable<BookDto> SearchBooks(string term)
        {
            var books = _unitOfWork.BookRepository.Search(term);

            return books.Select(b => new BookDto
            {
                BookId = b.Id,
                Title = b.Title,
                AuthorName = b.Author?.FullName ?? "Unknown",  
                BookImage = b.BookImage
            });
        }



    }
}