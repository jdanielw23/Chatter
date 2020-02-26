using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatter.Model;
using Chatter.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Services
{
    public class MessageService : IMessageService
    {
        private readonly DbContext _dbContext;

        public MessageService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Message>> GetMessages(int loggedInUserId, int otherUserId)
        {
            return await _dbContext.Set<Message>()
                .Where(m => m.FromUserId == loggedInUserId && m.ToUserId == otherUserId)
                .ToListAsync();
        }

        public async Task<Message> SendMessage(int userId, MessageModel messageModel)
        {
            var fromUser = await _dbContext.Set<User>().FindAsync(userId);
            var toUser = await _dbContext.Set<User>().FindAsync(messageModel.UserId);
            if (string.IsNullOrWhiteSpace(messageModel.Text) ||
                fromUser == null || toUser == null)
                throw new ArgumentException("Message model has invalid data");

            var message = new Message()
            {
                FromUserId = fromUser.Id,
                ToUserId = toUser.Id,
                Text = messageModel.Text,
                SentAt = messageModel.ClientSentAt,
                DeliveredAt = DateTimeOffset.Now,
            };
            await _dbContext.Set<Message>().AddAsync(message);
            await _dbContext.SaveChangesAsync();

            return message;
        }
    }

    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessages(int loggedInUserId, int otherUserId);
        Task<Message> SendMessage(int userId, MessageModel messageModel);
    }
}