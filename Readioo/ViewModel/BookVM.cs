using System.ComponentModel.DataAnnotations;

namespace Readioo.ViewModel
{
    public class BookVM
    {

        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Isbn { get; set; } = null!;
        [Required]
        public string Language { get; set; } = null!;
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int PagesCount { get; set; }
        [Required]
        public DateOnly PublishDate { get; set; }

        public string? MainCharacters { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        public IFormFile? BookImage { get; set; }
    }
}
