using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class MessageDAO : BaseDAO<Message>
    {
        public MessageDAO(SchoolDbContext context) : base(context) { }

        // 📌 Gửi tin nhắn
        public async Task<MessageDto> SendMessageAsync(Message msg)
        {
            msg.CreatedAt = DateTime.UtcNow;
            msg.IsRead = false;
            await _dbSet.AddAsync(msg);
            await _context.SaveChangesAsync();

            return new MessageDto
            {
                Id = msg.Id,
                SenderId = msg.SenderId,
                SenderName = (await _context.Users.FindAsync(msg.SenderId))?.FullName ?? "",
                ReceiverId = msg.ReceiverId,
                ReceiverName = (await _context.Users.FindAsync(msg.ReceiverId))?.FullName ?? "",
                Content = msg.Content,
                CreatedAt = msg.CreatedAt,
                IsRead = msg.IsRead ?? false
            };
        }

        // 📌 Lấy hội thoại giữa 2 user (có phân trang)
        public async Task<List<MessageDto>> GetConversationAsync(Guid user1, Guid user2, int page = 1, int pageSize = 20)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.SenderId == user1 && m.ReceiverId == user2) ||
                            (m.SenderId == user2 && m.ReceiverId == user1))
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.FullName,
                    ReceiverId = m.ReceiverId,
                    ReceiverName = m.Receiver.FullName,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt,
                    IsRead = m.IsRead ?? false
                })
                .ToListAsync();
        }

        // 📌 Đánh dấu 1 tin nhắn là đã đọc
        public async Task MarkAsReadAsync(Guid messageId)
        {
            var msg = await _dbSet.FindAsync(messageId);
            if (msg != null)
            {
                msg.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        // 📌 Lấy danh sách các cuộc trò chuyện (sidebar)
        public async Task<List<ConversationDto>> GetConversationsListAsync(Guid userId)
        {
            var conversations = await _dbSet
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => new
                {
                    OtherUserId = g.Key,
                    LastMessage = g.OrderByDescending(m => m.CreatedAt).FirstOrDefault(),
                    UnreadCount = g.Count(m => m.ReceiverId == userId && (m.IsRead == null || m.IsRead == false))
                })
                .ToListAsync();

            // Map sang ConversationDto
            var result = conversations.Select(c => new ConversationDto
            {
                OtherUserId = c.OtherUserId,
                OtherUserName = _context.Users.FirstOrDefault(u => u.Id == c.OtherUserId)!.FullName,
                LastMessage = c.LastMessage == null ? null : new MessageDto
                {
                    Id = c.LastMessage.Id,
                    SenderId = c.LastMessage.SenderId,
                    SenderName = c.LastMessage.Sender.FullName,
                    ReceiverId = c.LastMessage.ReceiverId,
                    ReceiverName = c.LastMessage.Receiver.FullName,
                    Content = c.LastMessage.Content,
                    CreatedAt = c.LastMessage.CreatedAt,
                    IsRead = c.LastMessage.IsRead ?? false
                },
                UnreadCount = c.UnreadCount
            }).OrderByDescending(c => c.LastMessage?.CreatedAt).ToList();

            return result;
        }

        // 📌 Lấy toàn bộ tin nhắn chưa đọc
        public async Task<List<MessageDto>> GetUnreadMessagesAsync(Guid userId)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == userId && (m.IsRead == null || m.IsRead == false))
                .OrderBy(m => m.CreatedAt)
                .Select(m => new MessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.FullName,
                    ReceiverId = m.ReceiverId,
                    ReceiverName = m.Receiver.FullName,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt,
                    IsRead = m.IsRead ?? false
                })
                .ToListAsync();
        }

        // 📌 Đánh dấu cả cuộc trò chuyện là đã đọc
        public async Task MarkConversationAsReadAsync(Guid senderId, Guid receiverId)
        {
            var unreadMessages = await _dbSet
               .Where(m => m.SenderId == senderId &&
                           m.ReceiverId == receiverId &&
                           (m.IsRead == null || m.IsRead == false))
               .ToListAsync();

            if (unreadMessages.Any())
            {
                foreach (var msg in unreadMessages)
                    msg.IsRead = true;

                await _context.SaveChangesAsync();
            }
        }

        // 📌 Xóa tin nhắn
        public async Task<bool> DeleteMessageAsync(Guid messageId)
        {
            var msg = await _dbSet.FindAsync(messageId);
            if (msg == null) return false;

            _dbSet.Remove(msg);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
