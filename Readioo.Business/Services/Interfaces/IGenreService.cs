using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DataTransferObjects.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Interfaces
{
    public interface IGenreService
    {
        // Returns the lightweight DTO for dropdowns/lists
        public List<GenreDto> GetAllGenres();

        // Returns the heavy DTO with Books and Authors for the details page
        public GenreDto GetGenreById(int id);

        public List<BookDto> GetBooksByGenre(int genreId);
    }
}
