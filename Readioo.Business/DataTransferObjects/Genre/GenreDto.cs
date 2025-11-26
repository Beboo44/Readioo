using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.DataTransferObjects.Genre
{
    public class GenreDto
    {
        public int Id { get; set; }
        public string GenreName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
