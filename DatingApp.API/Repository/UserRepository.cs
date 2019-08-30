using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(DataContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _dbSet.Include(i => i.Photos).ToListAsync();
        }

        public override async Task<User> Get(Expression<Func<User, bool>> filter)
        {
            return await _dbSet.Include(i => i.Photos).FirstOrDefaultAsync(filter);
        }

        public override async Task<IEnumerable<User>> Gets(Expression<Func<User, bool>> filter)
        {
            return await _dbSet.Include(i => i.Photos).Where(filter).ToListAsync();
        }

        public Task<User> GetUser(long id)
        {
            return Get(u => u.Id == id);
        }
    }
}