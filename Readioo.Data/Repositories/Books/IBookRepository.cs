using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.Books
{
    public interface IBookRepository:IGenericRepository<Book>
    {
        public IEnumerable<Book> GetAll();
        public IEnumerable<Book> GetAll(string name);
        public Book? GetBookWithDetails(int id);

        IEnumerable<Book> GetAllBooksWithDetails();

    }
}
