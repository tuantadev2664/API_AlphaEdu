using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class MessageRepository : IMessageRepository
    {
        public Task<bool> DeleteMessageAsync(Guid messageId)=> MessageDAO.DeleteMessageAsync(messageId);

        public Task<List<Message>> GetConversationAsync(Guid user1, Guid user2)=> MessageDAO.GetConversationAsync(user1, user2);

        public Task<List<Message>> GetLatestConversationsAsync(Guid userId)=>MessageDAO.GetLatestConversationsAsync(userId);

        public Task<List<Message>> GetUnreadMessagesAsync(Guid userId)=>MessageDAO.GetUnreadMessagesAsync(userId);

        public Task MarkAsReadAsync(Guid messageId) => MessageDAO.MarkAsReadAsync(messageId);   

        public Task MarkConversationAsReadAsync(Guid senderId, Guid receiverId)=> MessageDAO.MarkConversationAsReadAsync(senderId, receiverId);

        public Task<Message> SendMessageAsync(Message msg)=>MessageDAO.SendMessageAsync(msg);
    }
}
