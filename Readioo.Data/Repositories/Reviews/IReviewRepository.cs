using Readioo.DataAccess.Repositories.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readioo.Data.Models;
using Readioo.Models;

namespace Readioo.Data.Repositories.Reviews
{
    public interface IReviewRepository :IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByBookIdAsync(int bookId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
    }
}
