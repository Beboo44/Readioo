using Readioo.Business.DataTransferObjects.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(ReviewDto dto);
        Task UpdateReviewAsync(int reviewId, ReviewDto dto);
        Task DeleteReviewAsync(int reviewId);
        Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(int bookId);
    }
}
