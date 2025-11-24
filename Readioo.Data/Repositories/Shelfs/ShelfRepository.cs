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
    public class ShelfRepository : IShelfRepository
    {
        private readonly AppDbContext _dbContext;

        public ShelfRepository(AppDbContext dbContext)
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

        public async Task<List<Shelf>> GetUserShelvesAsync(int userId)
        {
            return await _dbContext.Shelves
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }
    }

}