using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;

namespace Readioo.Data.Repositories.Genres
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        public IEnumerable<Genre> GetAll();
        public Genre GetById(int id);
        public IQueryable<Genre> GetAllQueryable();
    }
}
