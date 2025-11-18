using Readioo.Data.Repositories.Authors;
using Readioo.Data.Repositories.Books;
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

        public int SaveChanges();

    }
}
