using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUser(long id);
    }
}