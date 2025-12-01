using Demo.DataAccess.Repositories.UoW;
using Microsoft.EntityFrameworkCore;
using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DataTransferObjects.Genre;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Classes
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AuthorDto? getAuthorById(int id)  // ✅ Changed return type to nullable
        {
            // 1. Get the Author entity with all related data
            var author = _unitOfWork.AuthorRepository.GetById(id);
            if (author == null)
            {
                return null;  // ✅ Now valid for nullable return type
            }

            // 2. Fetch the books for this author
            var authorBooks = _unitOfWork.BookRepository.GetAll()
                .Where(b => b.AuthorId == id)
                .Select(b => new BookDto
                {
                    BookId = b.Id,
                    Title = b.Title,
                    Isbn = b.Isbn,
                    Language = b.Language,
                    AuthorId = b.AuthorId,
                    PagesCount = b.PagesCount,
                    PublishDate = b.PublishDate,
                    MainCharacters = b.MainCharacters,
                    Rate = b.Rate,
                    Description = b.Description,
                    BookImage = b.BookImage
                })
                .ToList();

            // ✅ 3. Fetch genres for this author (from their books)
            var authorGenres = _unitOfWork.BookRepository.GetAllBooksWithDetails()
                .Where(b => b.AuthorId == id)
                .SelectMany(b => b.BookGenres)
                .Select(bg => bg.Genre)
                .Distinct()
                .Select(g => new GenreDto
                {
                    Id = g.Id,
                    GenreName = g.GenreName,
                    Description = g.Description
                })
                .ToList();

            // 4. Create the AuthorDto with books and genres
            AuthorDto authorDto = new AuthorDto()
            {
                AuthorId = author.Id,
                FullName = author.FullName,
                Bio = author.Bio,
                BirthCountry = author.BirthCountry,
                BirthCity = author.BirthCity,
                BirthDate = author.BirthDate,
                DeathDate = author.DeathDate,
                AuthorImage = author.AuthorImage,
                Books = authorBooks,
                Genres = authorGenres  // ✅ Now correct type
            };

            return authorDto;
        }

        public IEnumerable<AuthorDto> getAllAuthors()
        {
            var Authors = _unitOfWork.AuthorRepository.GetAll().
                Select(a=>getAuthorById(a.Id));

            return Authors;
        }
        public IEnumerable<Author> ShowAllAuthors()
        {
            var Authors = _unitOfWork.AuthorRepository.GetAll().ToList();
            return Authors;
        }

        public async Task CreateAuthor(AuthorCreatedDto author)
        {
            Author author1 = new Author()
            {
                FullName = author.FullName,
                Bio = author.Bio,
                BirthCity = author.BirthCity,
                BirthCountry = author.BirthCountry,
                BirthDate = author.BirthDate,
                DeathDate = author.DeathDate,
                AuthorImage = author.AuthorImage
            };
            _unitOfWork.AuthorRepository.Add(author1);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAuthor(int id, AuthorCreatedDto authorDto)
        {
            // Find the existing author
            var existingAuthor = _unitOfWork.AuthorRepository.GetById(id);

            if (existingAuthor == null)
            {
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            // Update properties
            existingAuthor.FullName = authorDto.FullName;
            existingAuthor.Bio = authorDto.Bio;
            existingAuthor.BirthCity = authorDto.BirthCity;
            existingAuthor.BirthCountry = authorDto.BirthCountry;
            existingAuthor.BirthDate = authorDto.BirthDate;
            existingAuthor.DeathDate = authorDto.DeathDate;
            existingAuthor.AuthorImage = authorDto.AuthorImage;

            // Save changes
            _unitOfWork.AuthorRepository.Update(existingAuthor);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAuthor(int id)
        {
            var existingAuthor = _unitOfWork.AuthorRepository.GetById(id);

            if (existingAuthor == null)
            {
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            _unitOfWork.AuthorRepository.Remove(existingAuthor);
            await _unitOfWork.CommitAsync();
        }
    }
}
