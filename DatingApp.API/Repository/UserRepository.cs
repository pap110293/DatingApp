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
            var query = _baseQuery.Where(i => i.Id != userParams.UserId);


            if (!string.IsNullOrWhiteSpace(userParams.Gender))
            {
                query = query.Where(i => i.Gender == userParams.Gender);
            }

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            query = query.Where(i => i.DateOfBirth >= minDob && i.DateOfBirth <= maxDob);


            // oder by
            switch (userParams.OrderBy)
            {
                case "created":
                    query = query.OrderByDescending(i => i.Created);
                    break;
                default:
                    query = query.OrderByDescending(i => i.LastActive);
                    break;
            }
            return await Gets(query, userParams.PageNumber, userParams.PageSize);
        }
    }
}