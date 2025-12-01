using BCrypt.Net;
using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DataTransferObjects.Shelves;
using Readioo.Business.DataTransferObjects.User;
using Readioo.Business.DTO;
using Readioo.Business.Services.Interfaces;
using Readioo.Data.Repositories.Books;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Classes
{
    public class UserService : IUserService
    {
        // Dependencies
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        // NOTE: In a real app, you would also inject a secure IPasswordHasher here.
        // private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork
                           /*, IPasswordHasher passwordHasher */)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            // _passwordHasher = passwordHasher;
        }

        // --- Implementation of the Registration Method ---

        public async Task<bool> RegisterUserAsync(UserRegistrationDto registrationDto)
        {
            if (_userRepository.ExistsByEmail(registrationDto.Email))
            {
                return false;
            }

            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

            var newUser = new User
            {
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                UserEmail = registrationDto.Email,
                UserPassword = hashedPassword, // Store the REAL HASHED password
                CreationDate = DateTime.UtcNow,
                Bio = null,
                City = null,
                Country = null,
            };

            _userRepository.Add(newUser);

           
            await _unitOfWork.CommitAsync(); // Use the async version of commit
          

            var defaultShelves = new List<Shelf>
            {
                new Shelf { ShelfName = "Currently Reading", UserId = newUser.Id },
                new Shelf { ShelfName = "Books Read",      UserId = newUser.Id },
                new Shelf { ShelfName = "Want to Read",          UserId = newUser.Id },
            };

            foreach (var shelf in defaultShelves)
            {
                await _unitOfWork.ShelfRepository.AddAsync(shelf);
            }

            await _unitOfWork.CommitAsync(); // Save shelves

            return true;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            
            return await Task.Run(() => _userRepository.GetByEmail(email));
        }

        public async Task<bool> VerifyUserCredentialsAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
            {
                return false; // User not found
            }

            return BCrypt.Net.BCrypt.Verify(password, user.UserPassword);
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDTO dto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
                return false;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Bio = dto.Bio;
            user.City = dto.City;
            user.Country = dto.Country;

            if (dto.UserImage != null)
                user.UserImage = dto.UserImage;

            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ShelfDto>> GetUserShelvesAsync(int userId)
        {
            var shelves = await _unitOfWork.ShelfRepository.GetUserShelvesAsync(userId);

            return shelves.Select(s => new ShelfDto
            {
                ShelfId = s.Id,
                ShelfName = s.ShelfName,
                UserId = s.UserId,
                BooksCount = s.BookShelves.Count(),
                BookShelves = s.BookShelves.Select(bs => new BookShelf
                {
                    BookId = bs.BookId,
                    ShelfId = bs.ShelfId
                }).ToList() ?? new List<BookShelf>()
            }).ToList();
        }

        public async Task<IEnumerable<ShelfWithBooksDto>> GetUserShelvesWithBooksAsync(int userId)
        {
            var shelves = await _unitOfWork.ShelfRepository.GetUserShelvesAsync(userId);

            return shelves.Select(s => new ShelfWithBooksDto
            {
                ShelfId = s.Id,
                ShelfName = s.ShelfName,
                Books = s.BookShelves.Select(bs => new BookDto
                {
                    BookId = bs.Book.Id,
                    Title = bs.Book.Title,
                    AuthorName = bs.Book.Author.FullName,
                    Isbn = bs.Book.Isbn,
                    Language = bs.Book.Language,
                    PagesCount = bs.Book.PagesCount,
                    PublishDate = bs.Book.PublishDate,
                    Description = bs.Book.Description,
                    Rate = bs.Book.Rate,
                    BookImage = bs.Book.BookImage
                }).ToList()
            }).ToList();
        }
    }
}