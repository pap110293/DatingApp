using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Repository
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
         Task<Like> GetLike(long userId, long recipientId);
    }
}