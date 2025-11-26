using Microsoft.EntityFrameworkCore;
using Readioo.Data.Data.Contexts;
using Readioo.DataAccess.Repositories.Generics;
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Repositories.Genres
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private readonly AppDbContext _dbContext;

        public GenreRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Genre> GetAll()
        {
            return _dbContext.Set<Genre>().ToList();
        }

        public Genre GetById(int id)
        {
            return _dbContext.Set<Genre>().FirstOrDefault(g => g.Id == id);
        }

        public IQueryable<Genre> GetAllQueryable()
        {
            return _dbContext.Genres.AsQueryable();
        }
    }
}
