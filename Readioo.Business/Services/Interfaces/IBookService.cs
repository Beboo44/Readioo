using Readioo.Business.DataTransferObjects.Book;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Readioo.Business.Services.Interfaces
{
    public interface IBookService
    {
        public BookDto? bookById(int id);
        public Task CreateBook(BookCreatedDto bookCreatedDto);

        public IEnumerable<BookDto> GetAllBooks();

        public Task UpdateBook(BookDto bookDto);

        public Task DeleteBook(int id);

        public IEnumerable<BookDto> GetRecentlyAddedBooks(int count);
        public Task<List<BookDto>> GetUserBooksAsync(int userId);
        public Task<int?> GetUserRating(int userId, int bookId);
        public Task SaveUserRating(int userId, int bookId, int rating);
        public Task UpdateBookAverageRating(int bookId);

        IEnumerable<BookDto> GetAllBooksWithGenres();

    }

}
