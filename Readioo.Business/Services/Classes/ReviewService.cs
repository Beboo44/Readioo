using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Review;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Classes
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddReviewAsync(ReviewDto dto)
        {
            var review = new Review
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                Rating = dto.Rating,
                ReviewText = dto.ReviewText,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _unitOfWork.ReviewRepository.Add(review);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateReviewAsync(int reviewId, ReviewDto dto)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);

            review.Rating = dto.Rating;
            review.ReviewText = dto.ReviewText;
            review.UpdatedAt = DateTime.Now;

            _unitOfWork.ReviewRepository.Update(review);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);

            _unitOfWork.ReviewRepository.Remove(review);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(int bookId)
        {
            var reviews = await _unitOfWork.ReviewRepository.GetReviewsByBookIdAsync(bookId);

            return reviews.Select(r => new ReviewDto
            {
                ReviewId = r.Id,
                UserId = r.UserId,
                BookId = r.BookId,
                Username = r.User?.FirstName+" "+ r.User?.LastName,
                Rating = r.Rating,
                ReviewText = r.ReviewText,
                CreatedAt = r.CreatedAt
            });
        }
    }

}
