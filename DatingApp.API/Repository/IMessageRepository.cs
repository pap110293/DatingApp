using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Repository
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<Message> GetMessage(long id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessagesThread(long userId, long recipientId);
    }
}