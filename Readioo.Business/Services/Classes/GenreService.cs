using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DataTransferObjects.Genre; // Ensure this is using the Public DTOs
using Readioo.Business.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Readioo.Models;
using System.Collections.Generic;
using System.Linq;

namespace Readioo.Business.Services.Classes
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get all books for a specific genre
        public List<BookDto> GetBooksByGenre(int genreId)
        {
            var genre = _unitOfWork.GenreRepository
                .GetAllQueryable()
                .Include(g => g.BookGenres)
                    .ThenInclude(bg => bg.Book)
                        .ThenInclude(b => b.Author)
                .FirstOrDefault(g => g.Id == genreId);

            if (genre == null)
            {
                return new List<BookDto>();
            }

            return genre.BookGenres
                .Select(bg => new BookDto
                {
                    BookId = bg.Book.Id,
                    Title = bg.Book.Title,
                    Isbn = bg.Book.Isbn,
                    Language = bg.Book.Language,
                    AuthorName = bg.Book.Author.FullName,
                    PagesCount = bg.Book.PagesCount,
                    PublishDate = bg.Book.PublishDate,
                    Description = bg.Book.Description,
                    Rate = bg.Book.Rate,
                    BookImage = bg.Book.BookImage
                })
                .ToList();
        }

        // Get genre by id
        public GenreDto GetGenreById(int id)
        {
            var genre = _unitOfWork.GenreRepository
                .GetAll()
                .FirstOrDefault(g => g.Id == id);

            if (genre == null) return null;

            return new GenreDto
            {
                Id = genre.Id,
                GenreName = genre.GenreName,
                Description = genre.Description
            };
        }

        // Get all genres
        public List<GenreDto> GetAllGenres()
        {
            return _unitOfWork.GenreRepository
                .GetAll()
                .Select(g => new GenreDto
                {
                    Id = g.Id,
                    GenreName = g.GenreName,
                    Description = g.Description
                })
                .ToList();
        }
    }
}