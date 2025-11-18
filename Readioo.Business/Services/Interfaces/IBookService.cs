using Readioo.Business.DataTransferObjects.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Readioo.Business.Services.Interfaces
{
    public interface IBookService
    {
        public BookDetailsDto? bookById(int id);
    }

}
