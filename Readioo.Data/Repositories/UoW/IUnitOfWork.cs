using Readioo.Data.Repositories.Authors;
using Readioo.Data.Repositories.Books;
using Readioo.Data.Repositories.BookShelves;
using Readioo.Data.Repositories.Genres;
using Readioo.Data.Repositories.Reviews;
using Readioo.Data.Repositories.Shelfs;
using Readioo.Data.Repositories.Shelfs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Repositories.UoW
{
    public interface IUnitOfWork
    {
        // Property for each repository

        public IBookRepository BookRepository { get; }
        public IUserRepository UserRepository { get; }
        public IAuthorRepository AuthorRepository { get; }
        public IShelfRepository ShelfRepository { get; }
        public IGenreRepository GenreRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        public IBookShelfRepository BookShelfRepository { get; }

        public int SaveChanges();
        Task CommitAsync();
        void Commit();
        Task<int> CompleteAsync();

    }
}
