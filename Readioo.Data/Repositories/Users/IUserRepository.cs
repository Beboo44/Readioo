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

        // 1. Check if a user with a specific email already exists (CRUCIAL for registration)
        //To use it for regestration
        bool ExistsByEmail(string email);

        // 2. Retrieve a user by email (CRUCIAL for login/authentication)
        User? GetByEmail(string email);

    }
}
