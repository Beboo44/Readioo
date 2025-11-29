using System.ComponentModel.DataAnnotations;

namespace Readioo.ViewModel
{
    public class RegisterVM
    {
        // Corresponds to UserEmail
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|yahoo\.com|outlook\.com)$",
        ErrorMessage = "Email domain must be valid")]
        public string Email { get; set; } = null!;

        // Corresponds to FirstName
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        // Corresponds to LastName (optional, nullable in the database)
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        // Corresponds to UserPassword
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&]).+$",
    ErrorMessage = "Password is weak: Password must contain at least one letter, number and symbol.")]
        public string Password { get; set; } = null!;

        // Used for confirmation—this field is for the UI only, not the database
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
