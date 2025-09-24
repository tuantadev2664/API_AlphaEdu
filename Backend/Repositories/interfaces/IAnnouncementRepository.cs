using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> CreateAnnouncementAsync(Announcement ann);

        // Cập nhật thông báo
        Task<Announcement?> UpdateAnnouncementAsync(Announcement updated);
        // Xoá thông báo
        Task<bool> DeleteAnnouncementAsync(Guid id);

        // Lấy thông báo theo lớp
         Task<List<Announcement>> GetAnnouncementsByClassAsync(Guid classId);

        // Lấy thông báo theo môn
        Task<List<Announcement>> GetAnnouncementsBySubjectAsync(Guid subjectId);

        // Lấy thông báo theo giáo viên (người gửi)
        Task<List<Announcement>> GetAnnouncementsBySenderAsync(Guid senderId);

        // Lấy thông báo khẩn
        Task<List<Announcement>> GetUrgentAnnouncementsAsync();

        // Lấy tất cả thông báo còn hiệu lực
        Task<List<Announcement>> GetActiveAnnouncementsAsync();
    }
}
