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
    public class UserRepository:GenericRepository<User>,IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<User> GetAll()
        {
            return _dbContext.Set<User>().ToList();
        }
        public IEnumerable<User> GetAll(string name)
        {
            return _dbContext.Set<User>().Where(x => (x.FirstName+" "+ x.LastName).Contains(name)).ToList();
        }
    }
}
