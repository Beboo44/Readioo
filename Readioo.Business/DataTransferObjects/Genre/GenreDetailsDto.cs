using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.DataTransferObjects.Genre
{
    public class GenreDetailsDto : GenreDto
    {
        public List<BookDto> Books { get; set; } = new List<BookDto>();
        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    }
}
