using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Review;
using Readioo.Business.DataTransferObjects.Genre; // ✅ 1. Add this using
using Readioo.Models;

namespace Readioo.Business.DataTransferObjects.Book
{
    public class BookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public string Isbn { get; set; } = null!;
        public string Language { get; set; } = null!;
        public int AuthorId { get; set; }
        public int PagesCount { get; set; }
        public DateOnly PublishDate { get; set; }
        public string? MainCharacters { get; set; }
        public decimal Rate { get; set; }
        public string Description { get; set; } = null!;
        public string? BookImage { get; set; }
        public string AuthorName { get; set; }
        public virtual AuthorDto Author { get; set; } = null!;

        // This is the list of strings (keep it if you use it elsewhere)
        public virtual ICollection<String> BookGenres { get; set; } = new List<String>();

        // ✅ 2. ADD THIS PROPERTY so the Index View works
        public List<GenreDto> Genres { get; set; } = new List<GenreDto>();

        public virtual ICollection<String> BookShelves { get; set; } = new List<String>();
        public virtual ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}