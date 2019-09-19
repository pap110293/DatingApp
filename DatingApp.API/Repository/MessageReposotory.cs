using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class MessageReposotory : BaseRepository<Message>, IMessageRepository
    {
        public MessageReposotory(DataContext context) : base(context)
        {
            _baseQuery = _baseQuery.Include(m => m.Sender).ThenInclude(u => u.Photos)
                .Include(m => m.Recipient).ThenInclude(u => u.Photos);
        }

        public async Task<Message> GetMessage(long id)
        {
            return await Get(i => i.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _baseQuery.AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    query = query.Where(m => m.RecipientId == messageParams.UserId);
                    break;
                case "Outbox":
                    query = query.Where(m => m.SenderId == messageParams.UserId);
                    break;
                default:
                    query = query.Where(m => m.RecipientId == messageParams.UserId && m.IsRead == false);
                    break;
            }

            query = query.OrderByDescending(m => m.MessageSent);
            return await PagedList<Message>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessagesThread(long userId, long recipientId)
        {
            var query = _baseQuery.AsQueryable();
            var messages = await query.Where(m => m.RecipientId == userId && m.SenderId == recipientId || m.RecipientId == recipientId && m.SenderId == userId)
                            .OrderBy(i => i.MessageSent)
                            .ToListAsync();
            return messages;
        }
    }
}