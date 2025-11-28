using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DataTransferObjects.Genre;
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

        // ✅ CHANGED: From List<string> to List<GenreDto>
        public List<GenreDto> Genres { get; set; } = new List<GenreDto>();

        // Books list
        public List<BookDto> Books { get; set; } = new List<BookDto>();
    }
}