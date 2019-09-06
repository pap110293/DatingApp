using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;

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
    }
}