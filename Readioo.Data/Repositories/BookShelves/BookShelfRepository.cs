using Microsoft.EntityFrameworkCore;
using Readioo.Data.Data.Contexts;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.BookShelves
{
    public class BookShelfRepository : IBookShelfRepository
    {
        private readonly AppDbContext _context;

        public BookShelfRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookShelf>> GetByShelfIdsAsync(List<int> shelfIds)
        {
            return await _context.BookShelves
                .Where(bs => shelfIds.Contains(bs.ShelfId))
                .ToListAsync();
        }

        public async Task AddAsync(BookShelf entity)
        {
            await _context.BookShelves.AddAsync(entity);
        }

        public async Task RemoveAsync(BookShelf entity)
        {
            _context.BookShelves.Remove(entity);
        }
        public IQueryable<BookShelf> Query()
        {
            return _context.BookShelves.AsQueryable();
        }
    }
}
