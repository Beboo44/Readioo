using Microsoft.EntityFrameworkCore;
using Readioo.Data.Data.Contexts;
using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.Books
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public BookRepository(AppDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Book> GetAll()
        {
            return _dbContext.Set<Book>().Include(b => b.Author).ToList();
        }

        public IEnumerable<Book> GetAll(string name)
        {
            return _dbContext.Set<Book>().Where(x => x.Title.Contains(name)).ToList();
        }
        public Book? GetBookWithDetails(int id)
        {
            return _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Include(b => b.Reviews).ThenInclude(r => r.User)
                .FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Book> Search(string term)
        {
            return _dbContext.Books
                .Where(b => b.Title.Contains(term))
                .Take(10)
                .ToList();
        }


    }
}
