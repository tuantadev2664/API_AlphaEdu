using BusinessObjects.Models;
using DataAccessObjects;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly MessageDAO _messDAO;

    public MessageRepository(SchoolDbContext context) : base(new MessageDAO(context))
    {
            _messDAO = new MessageDAO(context);
    }

        public Task<bool> DeleteMessageAsync(Guid messageId)=> _messDAO.DeleteMessageAsync(messageId);

        public Task<List<MessageDto>> GetConversationAsync(Guid user1, Guid user2, int page = 1, int pageSize = 20)=> _messDAO.GetConversationAsync(user1, user2, page, pageSize);

        public Task<List<ConversationDto>> GetConversationsListAsync(Guid userId)=>_messDAO.GetConversationsListAsync(userId);
        public Task<List<MessageDto>> GetUnreadMessagesAsync(Guid userId)=>_messDAO.GetUnreadMessagesAsync(userId);

        public Task MarkAsReadAsync(Guid messageId) => _messDAO.MarkAsReadAsync(messageId);

        public Task MarkConversationAsReadAsync(Guid senderId, Guid receiverId)=>_messDAO.MarkConversationAsReadAsync(senderId, receiverId);

        public Task<MessageDto> SendMessageAsync(Message msg)=>_messDAO.SendMessageAsync(msg);
    }
}
