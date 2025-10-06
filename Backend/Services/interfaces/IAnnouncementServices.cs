using BusinessObjects.Models;
using DataAccessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.interfaces
{
    public interface IAnnouncementServices : IService<Announcement>
    {
        Task<TeacherAnnouncementItem> AddAnnouncementAsync(CreateAnnouncementRequest request, Guid senderId);        // Cập nhật thông báo
        Task<TeacherAnnouncementItem?> UpdateAnnouncementAsync(UpdateAnnouncementRequest request);
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
