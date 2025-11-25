using Readioo.Business.DataTransferObjects.Book; // Make sure to reference the Book DTO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.DataTransferObjects.Author
{
    public class AuthorDto
    {
        public int AuthorId { get; set; }

        public string FullName { get; set; } = null!;

        public string Bio { get; set; } = null!;

        public string BirthCountry { get; set; } = null!;

        public string BirthCity { get; set; } = null!;

        public DateOnly BirthDate { get; set; }

        public DateOnly? DeathDate { get; set; }

        public string? AuthorImage { get; set; }

        public List<string> Genres { get; set; } = new();

        // --- NEW PROPERTY ---
        // This holds the list of books so the View can loop through them
        public List<BookDto> Books { get; set; } = new();
    }
}