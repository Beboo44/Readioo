using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DataTransferObjects.Genre; // Ensure this is using the Public DTOs
using Readioo.Business.Services.Interfaces;
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

        public IEnumerable<GenreDto> getAllGenres()
        {
            return _unitOfWork.GenreRepository.GetAll()
                .Select(g => new GenreDto
                {
                    Id = g.Id,
                    GenreName = g.GenreName,
                    Description = g.Description
                })
                .ToList();
        }

        public GenreDetailsDto getGenreById(int id)
        {
            var genre = _unitOfWork.GenreRepository.GetById(id);
            if (genre == null) return null;

            var genreBooks = _unitOfWork.BookRepository.GetAll()
                .Where(b => b.BookGenres.Any(bg => bg.GenreId == id))
                .Select(b => new BookDto
                {
                    BookId = b.Id,
                    Title = b.Title,
                    BookImage = b.BookImage,
                    Rate = b.Rate,
                    PublishDate = b.PublishDate,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author.FullName
                })
                .ToList();

            var genreAuthors = _unitOfWork.AuthorRepository.GetAll()
                .Where(a => a.AuthorGenres.Any(ag => ag.GenreId == id))
                .Select(a => new AuthorDto
                {
                    AuthorId = a.Id,
                    FullName = a.FullName,
                    AuthorImage = a.AuthorImage,
                    Bio = a.Bio
                })
                .ToList();

            return new GenreDetailsDto
            {
                Id = genre.Id,
                GenreName = genre.GenreName,
                Description = genre.Description,
                Books = genreBooks,
                Authors = genreAuthors
            };
        }
    }
}