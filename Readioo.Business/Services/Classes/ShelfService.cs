using Demo.DataAccess.Repositories.UoW;
using Microsoft.EntityFrameworkCore;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DTO;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Classes
{
    public class ShelfService : IShelfService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShelfService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ShelfDto>> GetAllShelves()
        {
            var shelves = _unitOfWork.ShelfRepository.GetAll().Select(a => new ShelfDto
            {
                ShelfId = a.Id,
                ShelfName = a.ShelfName,
                UserId = a.UserId,
                BooksCount = a.BookShelves.Count()
            });

            return shelves;
        }

        // --- NEW METHOD: Get shelves for specific user ---
        public async Task<IEnumerable<ShelfDto>> GetUserShelves(int userId)
        {
            return _unitOfWork.ShelfRepository.GetAll()
                .Where(s => s.UserId == userId)
                .Select(a => new ShelfDto
                {
                    ShelfId = a.Id,
                    ShelfName = a.ShelfName,
                    UserId = a.UserId,
                    BooksCount = a.BookShelves.Count()
                })
                .ToList();
        }

        public async Task<ShelfDto> GetShelfById(int id)
        {
            var shelf = _unitOfWork.ShelfRepository.GetById(id);
            if (shelf == null) return null;

            var shelfDto = new ShelfDto()
            {
                ShelfId = shelf.Id,
                ShelfName = shelf.ShelfName,
                UserId = shelf.UserId,
                BooksCount = shelf.BookShelves.Count
            };

            return shelfDto;
        }

        public async Task AddBook(int bookId, int shelfId, int favoriteId)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
            var shelf = await _unitOfWork.ShelfRepository.GetByIdAsync(shelfId);
            var favoriteShelf = await _unitOfWork.ShelfRepository.GetByIdAsync(favoriteId);

            if (book == null || shelf == null || favoriteShelf == null)
                throw new Exception("Book or Shelf Not Found");

            var mainEntry = new BookShelf { BookId = bookId, ShelfId = shelfId };
            var favoriteEntry = new BookShelf { BookId = bookId, ShelfId = favoriteId };

            if (!shelf.BookShelves.Any(bs => bs.BookId == bookId))
                shelf.BookShelves.Add(mainEntry);

            if (!favoriteShelf.BookShelves.Any(bs => bs.BookId == bookId))
                favoriteShelf.BookShelves.Add(favoriteEntry);

            await _unitOfWork.CommitAsync();
        }

        public List<BookDto> GetShelfBooks(int shelfId)
        {
            var shelf = _unitOfWork.ShelfRepository
                .GetAllQueryable()
                .Include(g => g.BookShelves)
                    .ThenInclude(bg => bg.Book)
                        .ThenInclude(b => b.Author)
                .FirstOrDefault(g => g.Id == shelfId);

            if (shelf == null)
            {
                return new List<BookDto>();
            }

            return shelf.BookShelves
                .Select(bg => new BookDto
                {
                    BookId = bg.Book.Id,
                    Title = bg.Book.Title,
                    Isbn = bg.Book.Isbn,
                    Language = bg.Book.Language,
                    AuthorName = bg.Book.Author.FullName,
                    PagesCount = bg.Book.PagesCount,
                    PublishDate = bg.Book.PublishDate,
                    Description = bg.Book.Description,
                    Rate = bg.Book.Rate,
                    BookImage = bg.Book.BookImage
                })
                .ToList();
        }

        public async Task<bool> MoveBookToShelfAsync(int userId, int bookId, string targetShelfName)
        {
            var userShelves = _unitOfWork.ShelfRepository.GetAllQueryable()
                .Where(s => s.UserId == userId)
                .Include(s => s.BookShelves)
                .ToList();

            var targetShelf = userShelves.FirstOrDefault(s =>
                s.ShelfName.Trim().Equals(targetShelfName.Trim(), StringComparison.OrdinalIgnoreCase));

            if (targetShelf == null) return false;

            var sourceShelf = userShelves.FirstOrDefault(s =>
                !s.ShelfName.Equals("Favorites", StringComparison.OrdinalIgnoreCase) &&
                s.BookShelves.Any(bs => bs.BookId == bookId));

            if (sourceShelf != null && sourceShelf.Id == targetShelf.Id) return true;

            if (sourceShelf != null)
            {
                var linkToRemove = sourceShelf.BookShelves.First(bs => bs.BookId == bookId);
                sourceShelf.BookShelves.Remove(linkToRemove);
                _unitOfWork.ShelfRepository.Update(sourceShelf);
            }

            if (!targetShelf.BookShelves.Any(bs => bs.BookId == bookId))
            {
                var newLink = new BookShelf { BookId = bookId, ShelfId = targetShelf.Id };
                targetShelf.BookShelves.Add(newLink);
                _unitOfWork.ShelfRepository.Update(targetShelf);
            }

            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}