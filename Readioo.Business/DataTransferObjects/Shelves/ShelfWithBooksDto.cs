using Readioo.Business.DataTransferObjects.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.DataTransferObjects.Shelves
{
    public class ShelfWithBooksDto
    {
        public int ShelfId { get; set; }
        public string ShelfName { get; set; }
        public List<BookDto> Books { get; set; }
    }
}
