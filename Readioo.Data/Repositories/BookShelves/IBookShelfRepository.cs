using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readioo.Data.Models;
using Readioo.Models;
namespace Readioo.Data.Repositories.BookShelves
{
    public interface IBookShelfRepository
    {
        Task<List<BookShelf>> GetByShelfIdsAsync(List<int> shelfIds);
        Task AddAsync(BookShelf entity);
        Task RemoveAsync(BookShelf entity);
        IQueryable<BookShelf> Query();

    }
}
