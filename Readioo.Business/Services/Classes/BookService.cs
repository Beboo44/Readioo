using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Book;
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
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BookDetailsDto? bookById(int id)
        {
            var book = _unitOfWork.BookRepository.GetById(id);
            if (book == null)
            {
                return null;
            }
            var bookDto = new BookDetailsDto()
            {
                BookId = book.BookId,
                Title = book.Title,
                Isbn = book.Isbn,
                Language = book.Language,
                AuthorId = book.AuthorId,
                PagesCount = book.PagesCount,
                PublishDate = book.PublishDate,
                MainCharacters = book.MainCharacters,
                Rate = book.Rate,
                Description = book.Description,
                BookImage = book.BookImage,

                BookGenres = book.BookGenres
                .Select(g=>g.Genre.GenreName)
                .ToList(),

                Reviews = book.Reviews
                    .Select(r => new ReviewDto
                    {
                        ReviewId = r.ReviewId,
                        UserId = r.UserId,
                        Username = r.User.FirstName + " " + r.User.LastName,
                        Rating = r.Rating,
                        ReviewText = r.ReviewText,
                        CreatedAt = r.CreatedAt
                        
                    })
                    .ToList()
            };
            return bookDto;
        }
    }
}
