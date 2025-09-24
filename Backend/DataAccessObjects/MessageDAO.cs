using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class MessageDAO
    {
     

        // Gửi tin nhắn
        public static async Task<Message> SendMessageAsync(Message msg)
        {

            using var _context = new SchoolDbContext();
            msg.CreatedAt = DateTime.UtcNow;
            msg.IsRead = false;
            await _context.Messages.AddAsync(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        // Lấy toàn bộ hội thoại giữa 2 người
        public static async Task<List<Message>> GetConversationAsync(Guid user1, Guid user2)
        {

            using var _context = new SchoolDbContext();
            return await _context.Messages
                .Where(m => (m.SenderId == user1 && m.ReceiverId == user2) ||
                            (m.SenderId == user2 && m.ReceiverId == user1))
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        // Đánh dấu 1 tin nhắn là đã đọc
        public static async Task MarkAsReadAsync(Guid messageId)
        {

            using var _context = new SchoolDbContext();
            var msg = await _context.Messages.FindAsync(messageId);
            if (msg != null)
            {
                msg.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        // 📌 Bổ sung thực tế

        // Lấy danh sách hội thoại (chỉ hiển thị người chat gần đây, giống Messenger)
        public static async Task<List<Message>> GetLatestConversationsAsync(Guid userId)
        {

            using var _context = new SchoolDbContext();
            return await _context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => g.OrderByDescending(m => m.CreatedAt).First())
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        // Lấy tất cả tin nhắn chưa đọc của 1 user
        public static async Task<List<Message>> GetUnreadMessagesAsync(Guid userId)
        {

            using var _context = new SchoolDbContext();
            return await _context.Messages
                .Where(m => m.ReceiverId == userId && m.IsRead == false)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        // Đánh dấu tất cả tin nhắn trong cuộc trò chuyện là đã đọc
        public static async Task MarkConversationAsReadAsync(Guid senderId, Guid receiverId)
        {

            using var _context = new SchoolDbContext();
            var unreadMessages = await _context.Messages
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

        // Xoá 1 tin nhắn (nếu cho phép)
        public static async Task<bool> DeleteMessageAsync(Guid messageId)
        {

            using var _context = new SchoolDbContext();
            var msg = await _context.Messages.FindAsync(messageId);
            if (msg == null) return false;

            _context.Messages.Remove(msg);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
