using Demo.DataAccess.Repositories.UoW;
using Readioo.Business.DataTransferObjects.Author;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.Services.Interfaces
{
    public interface IAuthorService
    {
        public IEnumerable<AuthorDto> getAllAuthors();
        public Task CreateAuthor(AuthorCreatedDto author);
        public IEnumerable<Author> ShowAllAuthors();


    }
}
