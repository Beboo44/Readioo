using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Readioo.Data.Data.Contexts; // Ensure you have this namespace for AppDbContext

namespace Readioo.Business.Services.Classes
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context; // Inject DbContext for complex queries

        public RecommendationService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<List<BookDto>> GetRecommendationsForUserAsync(string userId)
        {
            // Parse userId string to int before querying
            if (!int.TryParse(userId, out int parsedUserId))
            {
                // Fallback: Return Top Rated books if ID is invalid
                return _context.Books
                    .OrderByDescending(b => b.Rate)
                    .Take(6)
                    .Select(b => new BookDto
                    {
                        BookId = b.Id,
                        Title = b.Title,
                        BookImage = b.BookImage,
                        Rate = b.Rate,
                        AuthorName = b.Author.FullName,
                        AuthorId = b.AuthorId,
                        BookGenres = b.BookGenres.Select(bg => bg.Genre.GenreName).ToList()
                    })
                    .ToList();
            }

            // 1. Fetch Books reviewed by this user
            // We use _context.Books to enable .Include() support
            var userReadBooks = _context.Books
                .Where(b => b.Reviews.Any(r => r.UserId == parsedUserId))
                .Include(b => b.BookGenres)
                .Include(b => b.Reviews)
                .ToList();

            // 2. Identify "Liked" books (e.g., rated 4 or 5 stars)
            var likedBooks = userReadBooks
                .Where(b => b.Reviews.Any(r => r.UserId == parsedUserId && r.Rating >= 4))
                .ToList();

            // 3. Identify Preferences
            var likedGenreIds = likedBooks
                .SelectMany(b => b.BookGenres.Select(bg => bg.GenreId))
                .Distinct()
                .ToList();

            var likedAuthorIds = likedBooks
                .Select(b => b.AuthorId)
                .Distinct()
                .ToList();

            // ID list of books to EXCLUDE (already read)
            var readBookIds = userReadBooks.Select(b => b.Id).Distinct().ToList();

            List<Readioo.Models.Book> recommendedBooks;

            // 4. Generate Recommendations
            if (!likedGenreIds.Any())
            {
                // Cold Start: Return Top Rated books
                recommendedBooks = _context.Books
                    .Where(b => !readBookIds.Contains(b.Id))
                    .Include(b => b.Author)       // Include Author for DTO mapping
                    .Include(b => b.BookGenres)   // Include BookGenres -> Genre for DTO mapping
                        .ThenInclude(bg => bg.Genre)
                    .OrderByDescending(b => b.Rate)
                    .Take(6)
                    .ToList();
            }
            else
            {
                // Intelligent Search: Find unread books matching liked Genres or Authors
                recommendedBooks = _context.Books
                    .Where(b => !readBookIds.Contains(b.Id))
                    .Where(b =>
                        likedAuthorIds.Contains(b.AuthorId) ||
                        b.BookGenres.Any(bg => likedGenreIds.Contains(bg.GenreId))
                    )
                    .Include(b => b.Author)       // Include Author
                    .Include(b => b.BookGenres)   // Include Genres
                        .ThenInclude(bg => bg.Genre)
                    .OrderByDescending(b => likedAuthorIds.Contains(b.AuthorId))
                    .ThenByDescending(b => b.Rate)
                    .Take(6)
                    .ToList();
            }

            // 5. Map to DTO
            return recommendedBooks.Select(b => new BookDto
            {
                BookId = b.Id,
                Title = b.Title,
                BookImage = b.BookImage,
                Rate = b.Rate,
                AuthorName = b.Author?.FullName,
                AuthorId = b.AuthorId,
                BookGenres = b.BookGenres.Select(bg => bg.Genre.GenreName).ToList()
            }).ToList();
        }
    }
}