using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Repository
{
    public interface IBaseRepository<T> where T : class
    {
         void Add(T entity);
         void Delete(T entity);
         Task<bool> SaveAll();
         Task<IEnumerable<T>> GetAll();
         Task<T> Get(Expression<Func<T, bool>> filter);
         Task<PagedList<T>> Gets(IQueryable<T> source, int pageNumber, int pageSize);
         Task<bool> Any(Expression<Func<T, bool>> filter);
    }
}