using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Author;
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

        public IEnumerable<AuthorDto> getAllAuthors()
        {
            var Authors = _unitOfWork.AuthorRepository.GetAll().
                Select(a => new AuthorDto
                {
                    AuthorId = a.Id,
                    FullName = a.FullName,
                    Bio = a.Bio,
                    BirthCountry = a.BirthCountry,
                    BirthCity = a.BirthCity,
                    BirthDate = a.BirthDate,
                    DeathDate = a.DeathDate,
                    AuthorImage = a.AuthorImage
                });
                

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
    }
}
