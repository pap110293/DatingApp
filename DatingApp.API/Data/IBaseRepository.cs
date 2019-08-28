using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IBaseRepository<T> where T : class
    {
         void Add(T entity);
         void Delete(T entity);
         Task<bool> SaveAll();
         Task<IEnumerable<T>> All();
         Task<T> Get(Expression<Func<T, bool>> filter);
         Task<IEnumerable<T>> Gets(Expression<Func<T, bool>> filter);
    }
}