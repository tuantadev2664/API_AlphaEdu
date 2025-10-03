using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class MessageServices : Service<Message>, IMessageServices
    {
        private readonly IMessageRepository _repository;

        public MessageServices(SchoolDbContext context, IMessageRepository repository)
            : base(context) // Service<T> CRUD chung
        {
            _repository = repository;
        }

        public Task<bool> DeleteMessageAsync(Guid messageId)=> _repository.DeleteMessageAsync(messageId);
        public Task<List<MessageDto>> GetConversationAsync(Guid user1, Guid user2, int page = 1, int pageSize = 20)=>_repository.GetConversationAsync(user1, user2, page, pageSize);

        public Task<List<ConversationDto>> GetConversationsListAsync(Guid userId)=> _repository.GetConversationsListAsync(userId);

        public Task<List<MessageDto>> GetUnreadMessagesAsync(Guid userId)=>_repository.GetUnreadMessagesAsync(userId);

        public Task MarkAsReadAsync(Guid messageId)=>_repository.MarkAsReadAsync(messageId);

        public Task MarkConversationAsReadAsync(Guid senderId, Guid receiverId)=>_repository.MarkConversationAsReadAsync(senderId, receiverId);
        public Task<MessageDto> SendMessageAsync(Message msg)=>_repository.SendMessageAsync(msg);
    }
}
