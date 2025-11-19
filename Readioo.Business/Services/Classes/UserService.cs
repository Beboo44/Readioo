using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.User;
using Readioo.Business.Services.Interfaces;
using Readioo.Data.Repositories.Books;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

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
            // 1. Business Validation: Check if email already exists
            if (_userRepository.ExistsByEmail(registrationDto.Email))
            {
                return false;
            }

            // 2. Security: Hashing the Password (REPLACED PLACEHOLDER)
            // CRITICAL STEP: Use BCrypt.Net to securely hash the password.
            // HashPassword handles salting automatically.
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

            // 3. Mapping DTO to Entity Model and Initializing system/default fields
            var newUser = new User
            {
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                UserEmail = registrationDto.Email,
                UserPassword = hashedPassword, // Store the REAL HASHED password
                CreationDate = DateTime.UtcNow,
                Bio = null,
                City = null,
                Country = null
            };

            // 4. Data Persistence (Calling the Repository's Add method)
            _userRepository.Add(newUser);

            // 5. Commit Transaction
            // This method makes the database call to save the new user record.
            await _unitOfWork.CommitAsync(); // Use the async version of commit

            return true;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            // Use Task.Run to execute the synchronous repository method asynchronously,
            // preventing thread pool exhaustion in the web application.
            return await Task.Run(() => _userRepository.GetByEmail(email));
        }
        public async Task<bool> VerifyUserCredentialsAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
            {
                return false; // User not found
            }

            // CRITICAL: Use the BCrypt Verify method
            // Compares the plaintext password against the stored hash
            // The library handles the salting/hashing for verification.
            return BCrypt.Net.BCrypt.Verify(password, user.UserPassword);
        }

    }
}