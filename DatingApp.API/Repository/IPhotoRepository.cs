using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Repository
{
    public interface IPhotoRepository : IBaseRepository<Photo>
    {
        Task<Photo> GetPhoto(long id);
        Task<Photo> GetMainPhoto(long userId);
        Task<bool> Any(long userId, long id);
        Task<PagedList<Photo>> GetUnapprovedPhotos(PagingParams paging);
        Task<Photo> GetUnapprovedPhoto(long id);
    }
}