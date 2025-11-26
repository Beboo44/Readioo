using Microsoft.EntityFrameworkCore;
using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.Shelfs
{
    public interface IShelfRepository : IGenericRepository<Shelf>
    {
        Task AddAsync(Shelf shelf);
        Shelf? GetById(int id);
        Task<IEnumerable<Shelf>> GetUserShelvesAsync(int userId);

        public IEnumerable<Shelf> GetAll();
        public Shelf? GetByName(string ShelfName);
        public IQueryable<Shelf> GetAllQueryable();
    }
}