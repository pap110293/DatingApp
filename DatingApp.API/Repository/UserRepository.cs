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
        private readonly ILikeRepository _likeRepository;

        public UserRepository(DataContext context, ILikeRepository likeRepository) : base(context)
        {
            _baseQuery = _baseQuery.Include(i => i.Photos);
            _baseQuery = _baseQuery.Include(i => i.Likers);
            _baseQuery = _baseQuery.Include(i => i.Likees);
            _likeRepository = likeRepository;
        }

        public async Task<bool> Existed(long id)
        {
            return await Any(u => u.Id == id);
        }

        public async Task<User> GetUser(long id)
        {
            return await Get(u => u.Id == id);
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var query = await FilterUser(userParams);
            return await Gets(query, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IQueryable<User>> FilterUser(UserParams userParams)
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

            if (userParams.Likers)
            {
                var likerIds = await GetUserLikesId(userParams.UserId, userParams.Likers);
                query = query.Where(u => likerIds.Contains(u.Id));
            }

            if (userParams.Likees)
            {
                var likeeIds = await GetUserLikesId(userParams.UserId, userParams.Likers);
                query = query.Where(u => likeeIds.Contains(u.Id));
            }
            return query;
        }

        private async Task<IEnumerable<long>> GetUserLikesId(long id, bool likers)
        {
            var currentUser = await _baseQuery.FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
            {
                return currentUser.Likers.Select(i => i.LikerId);
            }

            return currentUser.Likees.Select(i => i.LikeeId);
        }
    }
}