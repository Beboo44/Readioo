using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DTO;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Interfaces
{
    public interface IShelfService
    {
        public Task <IEnumerable<ShelfDto>> GetAllShelves();
        public Task <ShelfDto> GetShelfById(int id);

        public Task AddBook(int shelfId, int bookId, int favoriteShelfId);
    }
}
