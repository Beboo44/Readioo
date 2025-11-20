using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.DataTransferObjects.Author
{
    public class AuthorCreatedDto
    {
        public string FullName { get; set; } = null!;

        public string Bio { get; set; } = null!;

        public string BirthCountry { get; set; } = null!;

        public string BirthCity { get; set; } = null!;

        public DateOnly BirthDate { get; set; }

        public DateOnly? DeathDate { get; set; }

        public string? AuthorImage { get; set; }

        public List<string> Genres { get; set; } = new();
    }
}
