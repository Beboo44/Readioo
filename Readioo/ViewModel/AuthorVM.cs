using Microsoft.AspNetCore.Http;
using Readioo.Models;
using System.ComponentModel.DataAnnotations;

namespace Readioo.ViewModel
{
    public class AuthorVM
    {
        // Id for links/actions and binding in edit form
        public int AuthorId { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Bio { get; set; } = null!;

        [Required]
        public string BirthCountry { get; set; } = null!;
        [Required]
        public string BirthCity { get; set; } = null!;
        [Required]
        public DateOnly BirthDate { get; set; }

        public DateOnly? DeathDate { get; set; }

        // For upload
        public IFormFile? AuthorImage { get; set; }

        // Public path (or URL) to stored image to render in views
        public string? AuthorImagePath { get; set; }
    }
}
