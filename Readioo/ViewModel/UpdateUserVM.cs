using System.ComponentModel.DataAnnotations;

namespace Readioo.ViewModel
{
    public class UpdateUserVM
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [StringLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
        [Display(Name = "Biography")]
        public string? Bio { get; set; }

        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string? City { get; set; }

        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
        public string? Country { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile? UserImageFile { get; set; }
    }
}
