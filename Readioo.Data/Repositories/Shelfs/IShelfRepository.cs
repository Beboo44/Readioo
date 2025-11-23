using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.Shelfs
{
    public interface IShelfRepository
    {
        Task AddAsync(Shelf shelf);
        Shelf? GetById(int id);
        Task<List<Shelf>> GetUserShelvesAsync(int userId);
    }
}