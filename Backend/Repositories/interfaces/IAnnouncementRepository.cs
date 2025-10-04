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
        Task<Announcement> AddAnnouncementAsync(Announcement ann);
        // Cập nhật thông báo
        Task<Announcement?> UpdateAnnouncementAsync(Announcement updated);
        Task<bool> DeleteAnnouncementAsync(Guid id);
        // Lấy thông báo theo lớp
        Task<List<Announcement>> GetAnnouncementsByClassAndTermAsync(Guid classId, Guid termId, Guid academicYearId);

        // Lấy thông báo theo môn
        Task<List<Announcement>> GetAnnouncementsByClassAsync(Guid classId);
        // Lấy thông báo theo giáo viên (người gửi)
        Task<List<Announcement>> GetAnnouncementsBySubjectAsync(Guid subjectId);
        // Lấy thông báo khẩn
        Task<List<Announcement>> GetAnnouncementsByTeacherAsync(Guid teacherId);
        // Lấy tất cả thông báo còn hiệu lực
        Task<List<Announcement>> GetUrgentAnnouncementsAsync();
        Task<List<Announcement>> GetActiveAnnouncementsAsync();
        Task<Announcement?> GetByIdAsync(object id);
    }
}
