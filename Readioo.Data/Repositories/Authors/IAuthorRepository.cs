using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.Authors
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        public IEnumerable<Author> GetAll();
        public IEnumerable<Author> GetAll(string name);
    }
}
