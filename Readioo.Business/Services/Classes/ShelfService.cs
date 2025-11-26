using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DTO;
using Readioo.Business.Services.Interfaces;
using Readioo.Data.Repositories.Shelfs;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task <ShelfDto> GetShelfByName(string ShelfName)
        {
            var shelf = _unitOfWork.ShelfRepository.GetByName(ShelfName);
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

            bool favoriteExists = await _unitOfWork.BookShelfRepository.ExistsAsync(favoriteId, bookId);
            bool shelfExists = await _unitOfWork.BookShelfRepository.ExistsAsync(shelfId, bookId);

            if (!favoriteExists)
            {
                _unitOfWork.BookShelfRepository.Add(favoriteEntry);

                favoriteShelf.BookShelves.Add(favoriteEntry);
                book.BookShelves.Add(favoriteEntry);
            }

            if (!shelfExists)
            {
                _unitOfWork.BookShelfRepository.Add(mainEntry);

                shelf.BookShelves.Add(mainEntry);
                book.BookShelves.Add(mainEntry);
            }

            await _unitOfWork.CommitAsync();
        }


    }

}
