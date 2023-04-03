using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _dbSet;
        protected IQueryable<T> _baseQuery;

        public BaseRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _baseQuery = _dbSet;
        }

        public IQueryable<T> BaseQuery
        {
            get
            {
                return _baseQuery;
            }
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task<T> Get(Expression<System.Func<T, bool>> filter)
        {
            return await _baseQuery.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _baseQuery.ToListAsync();
        }

        public virtual async Task<PagedList<T>> Gets(IQueryable<T> source, int pageNumber, int pageSize)
        {
            return await PagedList<T>.CreateAsync(source, pageNumber, pageSize);
        }

        public virtual async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}