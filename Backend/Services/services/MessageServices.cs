using BusinessObjects.Models;
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class MessageServices : IMessageServices
    {
        private readonly IMessageRepository repo;

        public MessageServices(IMessageRepository repository) { repo = repository; }
        public Task<bool> DeleteMessageAsync(Guid messageId)=>repo.DeleteMessageAsync(messageId);

        public Task<List<Message>> GetConversationAsync(Guid user1, Guid user2)=>repo.GetConversationAsync(user1, user2);

        public Task<List<Message>> GetLatestConversationsAsync(Guid userId)=>repo.GetLatestConversationsAsync(userId);

        public Task<List<Message>> GetUnreadMessagesAsync(Guid userId)=>repo.GetUnreadMessagesAsync(userId);

        public Task MarkAsReadAsync(Guid messageId)=>repo.MarkAsReadAsync(messageId);

        public Task MarkConversationAsReadAsync(Guid senderId, Guid receiverId)=>repo.MarkConversationAsReadAsync(senderId, receiverId);

        public Task<Message> SendMessageAsync(Message msg)=>repo.SendMessageAsync(msg);
    }
}
