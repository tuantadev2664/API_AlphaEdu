using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class MessageDAO : BaseDAO<Message>
    {
        public MessageDAO(SchoolDbContext context) : base(context)
        {
        }

        // 📌 Gửi tin nhắn
        public async Task<Message> SendMessageAsync(Message msg)
        {
            msg.CreatedAt = DateTime.UtcNow;
            msg.IsRead = false;
            await _dbSet.AddAsync(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        // 📌 Lấy hội thoại giữa 2 user (phân trang giống Messenger)
        public async Task<List<Message>> GetConversationAsync(
            Guid user1, Guid user2,
            int page = 1, int pageSize = 20)
        {
            return await _dbSet
                .Where(m => (m.SenderId == user1 && m.ReceiverId == user2) ||
                            (m.SenderId == user2 && m.ReceiverId == user1))
                .OrderByDescending(m => m.CreatedAt)   // Tin mới trước
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

        // 📌 Lấy danh sách hội thoại gần đây (giống Messenger sidebar)
        public async Task<List<Message>> GetLatestConversationsAsync(Guid userId)
        {
            return await _dbSet
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => g.OrderByDescending(m => m.CreatedAt).First())
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        // 📌 Lấy toàn bộ tin nhắn chưa đọc
        public async Task<List<Message>> GetUnreadMessagesAsync(Guid userId)
        {
            return await _dbSet
                .Where(m => m.ReceiverId == userId && m.IsRead == false)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        // 📌 Đánh dấu cả cuộc trò chuyện là đã đọc
        public async Task MarkConversationAsReadAsync(Guid senderId, Guid receiverId)
        {
            var unreadMessages = await _dbSet
               .Where(m => m.SenderId == senderId &&
            m.ReceiverId == receiverId &&
            m.IsRead == false)
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
