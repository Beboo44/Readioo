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
        public IEnumerable<GenreDto> getAllGenres();

        // Returns the heavy DTO with Books and Authors for the details page
        public GenreDetailsDto getGenreById(int id);
    }
}
