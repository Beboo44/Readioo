using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.BookShelves
{
    public interface IBookShelfRepository : IGenericRepository<BookShelf>
    {
        public IEnumerable<BookShelf> GetAll();
        public Task<bool> ExistsAsync(int shelfId, int bookId);
    }
}
