using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.Books
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public IEnumerable<User> GetAll();
        public IEnumerable<User> GetAll(string name);
    }
}
