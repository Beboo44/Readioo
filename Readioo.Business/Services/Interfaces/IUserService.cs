using Readioo.Business.DataTransferObjects.User;
using Readioo.Business.DTO;
using Readioo.Data.Models;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegistrationDto registrationDto);
        Task<List<ShelfDto>> GetUserShelvesAsync(int userId);
        // Get user for sign in
        Task<User?> GetUserByEmailAsync(string email);
        //for password hashing
        Task<bool> VerifyUserCredentialsAsync(string email, string password);
        Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDTO dto);
        Task<User> GetUserByIdAsync(int id);
        


    }
}
