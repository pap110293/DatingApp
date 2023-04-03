using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class LikeRepository : BaseRepository<Like>, ILikeRepository
    {
        public LikeRepository(DataContext context) : base(context)
        {
            
        }

        public async Task<Like> GetLike(long userId, long recipientId)
        {
            return await Get(i=> i.LikerId == userId && i.LikeeId == recipientId);
        }
    }
}