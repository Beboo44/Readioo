using Readioo.Models;
using System.ComponentModel.DataAnnotations;

namespace Readioo.ViewModel
{
    public class AuthorVM
    {
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

        public IFormFile? AuthorImage { get; set; }
    }
}
