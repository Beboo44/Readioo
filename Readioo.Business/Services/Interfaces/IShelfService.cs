using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Interfaces
{
    public interface IShelfService
    {
        Task<IEnumerable<ShelfDto>> GetAllShelves();
        Task<ShelfDto> GetShelfById(int id);
        Task AddBook(int bookId, int shelfId, int favoriteId);
        List<BookDto> GetShelfBooks(int shelfId);
        Task<bool> MoveBookToShelfAsync(int userId, int bookId, string shelfName);
        // --- ADD THIS NEW METHOD ---
        Task<IEnumerable<ShelfDto>> GetUserShelves(int userId);
    }
}