using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                    query = query.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDeleted == false);
                    break;
                case "Outbox":
                    query = query.Where(m => m.SenderId == messageParams.UserId && m.SenderDeleted == false);
                    break;
                default:
                    query = query.Where(m => m.RecipientId == messageParams.UserId && m.IsRead == false && m.RecipientDeleted == false);
                    break;
            }

            query = query.OrderByDescending(m => m.MessageSent);
            return await PagedList<Message>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessagesThread(long userId, long recipientId)
        {
            var query = _baseQuery.AsQueryable();
            
            Expression<Func<Message,bool>> filter = m => 
                m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false 
                || 
                m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false;

            var messages = await query.Where(filter)
                            .OrderByDescending(i => i.MessageSent)
                            .ToListAsync();
            return messages;
        }
    }
}