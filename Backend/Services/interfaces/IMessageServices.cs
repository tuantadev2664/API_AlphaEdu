using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.interfaces
{
    public interface IMessageServices
    {
        Task<Message> SendMessageAsync(Message msg);

        // Lấy toàn bộ hội thoại giữa 2 người
        Task<List<Message>> GetConversationAsync(Guid user1, Guid user2);

        // Đánh dấu 1 tin nhắn là đã đọc
        Task MarkAsReadAsync(Guid messageId);

        // 📌 Bổ sung thực tế

        // Lấy danh sách hội thoại (chỉ hiển thị người chat gần đây, giống Messenger)
        Task<List<Message>> GetLatestConversationsAsync(Guid userId);

        // Lấy tất cả tin nhắn chưa đọc của 1 user
        Task<List<Message>> GetUnreadMessagesAsync(Guid userId);

        // Đánh dấu tất cả tin nhắn trong cuộc trò chuyện là đã đọc
        Task MarkConversationAsReadAsync(Guid senderId, Guid receiverId);

        // Xoá 1 tin nhắn (nếu cho phép)
        Task<bool> DeleteMessageAsync(Guid messageId);
    }
}
