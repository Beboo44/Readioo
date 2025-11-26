using Microsoft.EntityFrameworkCore;
using Readioo.Data.Data.Contexts;
using Readioo.Data.Repositories.Authors;
using Readioo.Data.Repositories.Books;
using Readioo.Data.Repositories.Genres;
using Readioo.Data.Repositories.Reviews;
using Readioo.Data.Repositories.Shelfs;
using Readioo.DataAccess.Repositories.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Demo.DataAccess.Repositories.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private Lazy<IBookRepository> _bookRepository;
        private Lazy<IAuthorRepository> _authorRepository;
        private Lazy<IUserRepository> _userRepository;
        private Lazy<IShelfRepository> _shelfRepository;
        private Lazy<IGenreRepository> _genreRepository;
        private Lazy<IReviewRepository> _reviewRepository;

        readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _bookRepository   = new Lazy<IBookRepository>(() => new BookRepository(_dbContext));
            _authorRepository = new Lazy<IAuthorRepository>(() => new AuthorRepository(_dbContext));
            _userRepository   = new Lazy<IUserRepository>(() => new UserRepository(_dbContext));
            _shelfRepository  = new Lazy<IShelfRepository>(() => new ShelfRepository(_dbContext));
            _genreRepository = new Lazy<IGenreRepository>(() => new GenreRepository(_dbContext));
            _reviewRepository = new Lazy<IReviewRepository>(() => new ReviewRepository(_dbContext));

        }

        public IAuthorRepository AuthorRepository => _authorRepository.Value;
        public IBookRepository BookRepository => _bookRepository.Value;
        public IUserRepository UserRepository => _userRepository.Value;
        public IShelfRepository ShelfRepository => _shelfRepository.Value;
        public IGenreRepository GenreRepository => _genreRepository.Value;
        public IReviewRepository ReviewRepository => _reviewRepository.Value;

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        // Existing synchronous method
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Implements the asynchronous save operation required by IUnitOfWork.
        /// </summary>
        public async Task CommitAsync()
        {
            // This is the essential fix: using the EF Core asynchronous method.
            await _dbContext.SaveChangesAsync();
        }
        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
