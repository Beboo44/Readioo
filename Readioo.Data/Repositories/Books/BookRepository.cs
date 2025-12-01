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

        public IEnumerable<Book> GetAllBooksWithDetails()
        {
            return _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .ToList();
        }
        public UserBook? GetUserBookRating(int userId, int bookId)
        {
            return _dbContext.Set<UserBook>()
                .FirstOrDefault(ub => ub.UserId == userId && ub.BookId == bookId);
        }

        public async Task SaveUserRating(int userId, int bookId, int rating)
        {
            var userBook = _dbContext.Set<UserBook>()
                .FirstOrDefault(ub => ub.UserId == userId && ub.BookId == bookId);

            if (userBook == null)
            {
                userBook = new UserBook
                {
                    UserId = userId,
                    BookId = bookId,
                    UserRating = rating
                };
                _dbContext.Set<UserBook>().Add(userBook);
            }
            else
            {
                userBook.UserRating = rating;
            }

            await _dbContext.SaveChangesAsync();
        }
        public decimal CalculateAverageRating(int bookId)
        {
            var ratings = _dbContext.Set<UserBook>()
                .Where(ub => ub.BookId == bookId && ub.UserRating.HasValue)
                .Select(ub => ub.UserRating.Value)
                .ToList();

            if (!ratings.Any())
                return 0m;

            return (decimal)ratings.Average();
        }


        public IEnumerable<Book> SearchBooks(string term)
        {
            return _dbContext.Books
                .Include(b => b.Author )
                //.Include(b=>b.BookGenres)
                .Where(b => b.Title.Contains(term) || b.Author.FullName.Contains(term))
                .ToList();
        }

        public Book? GetById(int id)
        {
            return _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Include(b => b.Reviews).ThenInclude(r => r.User)
                .FirstOrDefault(b => b.Id == id);
        }
    }
}
