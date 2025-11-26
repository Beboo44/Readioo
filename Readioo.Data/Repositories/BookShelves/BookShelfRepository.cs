using Microsoft.EntityFrameworkCore;
using Readioo.Data.Data.Contexts;
using Readioo.Data.Repositories.Books;
using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.BookShelves
{
    public class BookShelfRepository : GenericRepository<BookShelf>, IBookShelfRepository
    {
        private readonly AppDbContext _dbContext;

        public BookShelfRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BookShelf> GetAll()
        {
            return _dbContext.Set<BookShelf>().ToList();
        }

        public async Task<bool> ExistsAsync(int shelfId, int bookId)
        {
            return await _dbContext.BookShelves
                .AnyAsync(a => a.ShelfId == shelfId && a.BookId == bookId);
        }

    }
}
