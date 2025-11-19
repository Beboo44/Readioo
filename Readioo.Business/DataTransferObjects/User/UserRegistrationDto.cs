using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Readioo.Business.DataTransferObjects.User
{
    public class UserRegistrationDto
    {
        // Corresponds to UserEmail in your model
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        // Corresponds to FirstName in your model
        [Required]
        public string FirstName { get; set; } = null!;

        // Corresponds to LastName (optional, nullable in your model)
        public string? LastName { get; set; }

        // Corresponds to UserPassword in your model (will be hashed by the Service)
        [Required]
        public string Password { get; set; } = null!;

        // Used by the Service to confirm the password matches (no need to save this)
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;

    }
}
