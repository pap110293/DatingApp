using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class PhotoReposotory : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoReposotory(DataContext context) : base(context)
        {
        }

        public async Task<Photo> GetMainPhoto(long userId)
        {
            return await Get(p => p.UserId == userId && p.IsMain);
        }

        public async Task<Photo> GetPhoto(long id)
        {
            return await Get(p => p.Id == id);
        }

        public async Task<bool> Any(long userId, long id)
        {
            return await Any(i => i.UserId == userId && i.Id == id);
        }

        public async Task<PagedList<Photo>> GetUnapprovedPhotos(PagingParams paging)
        {
            var query = BaseQuery.IgnoreQueryFilters().Where(i => i.IsApproved == false).OrderBy(i => i.DateAdded);
            return await Gets(query,paging.PageNumber, paging.PageSize);
        }

        public async Task<Photo> GetUnapprovedPhoto(long id)
        {
            var query = BaseQuery.IgnoreQueryFilters();
            return await query.FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}