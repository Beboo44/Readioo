using Readioo.Data.Data.Contexts;
using Readioo.Data.Repositories.Books;
using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.Authors
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly AppDbContext _dbContext;

        public AuthorRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Author> GetAll()
        {
            return _dbContext.Set<Author>().ToList();
        }

        public IEnumerable<Author> GetAll(string name)
        {
            return _dbContext.Set<Author>().Where(x => x.FullName.Contains(name)).ToList();
        }
    }
}
