using Readioo.Business.DataTransferObjects.Book;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Interfaces
{
    public interface IRecommendationService
    {
        // Get personalized recommendations for a logged-in user
        Task<List<BookDto>> GetRecommendationsForUserAsync(string userId);
    }
}
