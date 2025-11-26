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

namespace Readioo.Data.Repositories.Shelfs
{
    public class ShelfRepository : GenericRepository<Shelf>, IShelfRepository
    {
        private readonly AppDbContext _dbContext;

        public ShelfRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Shelf shelf)
        {
            await _dbContext.Shelves.AddAsync(shelf);
        }

        public Shelf? GetById(int id)
        {
            return _dbContext.Shelves.FirstOrDefault(s => s.Id == id);
        }
        public Shelf? GetByName(string ShelfName)
        {
            return _dbContext.Shelves.FirstOrDefault(s => s.ShelfName == ShelfName);
        }

        public async Task<IEnumerable<Shelf>> GetUserShelvesAsync(int userId)
        {
            return await _dbContext.Shelves
                .Where(s => s.UserId == userId)
                .Include(s => s.BookShelves)
                .ToListAsync();
        }
        public IEnumerable<Shelf> GetAll()
        {
            return _dbContext.Set<Shelf>().ToList();
        }
        public IQueryable<Shelf> GetAllQueryable()
        {
            return _dbContext.Shelves.AsQueryable();
        }
    }

}