using System.ComponentModel.DataAnnotations;

namespace Readioo.ViewModel
{
    public class BookVM
    {

        

        [Required]
        [Display(Name = "Book Name")]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "Book ISBN")]
        [RegularExpression(@"^(0|[1-9][0-9]*)$",
            ErrorMessage = "ISBN should contain only numbers and no leading zeros.")]
        public string Isbn { get; set; } = null!;

        [Required]
        public string Language { get; set; } = null!;

        [Required]
        [Display(Name = "Book's Author")]
        public int AuthorId { get; set; }

        public virtual ICollection<string> BookGenres { get; set; } = new List<string>();

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Pages count must be greater than zero.")]
        public int PagesCount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly PublishDate { get; set; }

        [Display(Name = "Main Characters")]
        [StringLength(500, ErrorMessage = "Main characters list cannot exceed 500 characters.")]
        public string? MainCharacters { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 20,
            ErrorMessage = "Description must be between 20 and 2000 characters.")]
        public string Description { get; set; } = null!;

        [Display(Name = "Book Image")]
        public IFormFile? BookImage { get; set; }

        public string? BookImg { get; set; }
    }
}
