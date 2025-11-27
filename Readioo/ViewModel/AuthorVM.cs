using Microsoft.AspNetCore.Http;
using Readioo.Models;
using System.ComponentModel.DataAnnotations;

namespace Readioo.ViewModel
{
    public class AuthorVM
    {
        // Id for links/actions and binding in edit form
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters.")]
        [Display(Name = "Author Full-Name")]
        public string FullName { get; set; } = null!;

        [StringLength(5000, ErrorMessage = "Bio cannot exceed 5000 characters.")]
        [Display(Name = "Biography")]
        public string Bio { get; set; } = null!;

        [Required(ErrorMessage = "Birth country is required.")]
        [StringLength(100)]
        [Display(Name = "Country of Birth")]
        public string BirthCountry { get; set; } = null!;

        [Required(ErrorMessage = "Birth city is required.")]
        [StringLength(100)]
        [Display(Name = "City of Birth")]
        public string BirthCity { get; set; } = null!;

        [Required(ErrorMessage = "Birthdate is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Birthdate")]
        public DateOnly BirthDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Death Date")]
        public DateOnly? DeathDate { get; set; }

        // For file upload
        [Display(Name = "Author Image")]
        public IFormFile? AuthorImage { get; set; }

        public string? AuthorImagePath { get; set; }
    }
}
