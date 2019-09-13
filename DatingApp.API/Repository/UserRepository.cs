using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(DataContext context) : base(context)
        {
            _baseQuery = _baseQuery.Include(i => i.Photos);
        }

        public async Task<User> GetUser(long id)
        {
            return await Get(u => u.Id == id);
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            return await Gets(_baseQuery, userParams.PageNumber, userParams.PageSize);
        }
    }
}