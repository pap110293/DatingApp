using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUser(long id);
        Task<bool> Existed(long id);
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetEditUser(long id);

    }
}