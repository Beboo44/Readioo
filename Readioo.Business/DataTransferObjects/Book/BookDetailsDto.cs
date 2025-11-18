using Readioo.Business.DataTransferObjects.Review;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.DataTransferObjects.Book
{
    public class BookDetailsDto
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

        public virtual Author Author { get; set; } = null!;

        public virtual ICollection<String> BookGenres { get; set; } = new List<String>();
        public virtual ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();

    }
}
