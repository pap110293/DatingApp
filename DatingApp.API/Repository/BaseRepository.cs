using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository (DataContext context){
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void Add (T entity) {
            _dbSet.Add(entity);
        }

        public virtual void Delete (T entity) {
            _dbSet.Remove(entity);
        }

        public virtual async Task<T> Get (Expression<System.Func<T, bool>> filter) {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<IEnumerable<T>> GetAll () {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> Gets(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public virtual async Task<bool> SaveAll () {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}