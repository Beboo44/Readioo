using Readioo.Business.DataTransferObjects.User;
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

        // Get user for sign in
        Task<User?> GetUserByEmailAsync(string email);
        //for password hashing
        Task<bool> VerifyUserCredentialsAsync(string email, string password);
    }
}
