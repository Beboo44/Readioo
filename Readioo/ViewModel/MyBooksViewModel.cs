using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.DataTransferObjects.Shelves;

namespace Readioo.ViewModel

{
    public class MyBooksViewModel
    {
        public IEnumerable<ShelfWithBooksDto> Shelveswithbook { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
