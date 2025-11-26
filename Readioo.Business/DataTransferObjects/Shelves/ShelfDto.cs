using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.DTO
{
    public class ShelfDto
    {
        public int ShelfId { get; set; }
        public string ShelfName { get; set; }
        public int UserId { get; set; }
        public int BooksCount { get; set; }
        public virtual ICollection<BookShelf> BookShelves { get; set; } 

    }
}