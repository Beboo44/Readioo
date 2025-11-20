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

        // --- User-Specific Methods from IUserRepository ---

        /// <summary>
        /// Checks if a User with the given email already exists in the database.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if a user exists, false otherwise.</returns>
        public bool ExistsByEmail(string email)
        {
            // Use Any() for maximum efficiency as it only checks for existence (SELECT 1...)
            // Ensure email comparison is case-insensitive if your database supports it (ToLowerInvariant for safety)
            return _dbContext.Set<User>()
                             .Any(u => u.UserEmail.ToLower() == email.ToLower());
        }

        /// <summary>
        /// Retrieves a User entity based on their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>The User entity or null if not found.</returns>
        public User? GetByEmail(string email)
        {
            // Use FirstOrDefault() to retrieve a single entity, or null if not found.
            return _dbContext.Set<User>()
                             .FirstOrDefault(u => u.UserEmail.ToLower() == email.ToLower());
        }
    }
}
