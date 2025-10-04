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
    public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
    {
        private readonly AnnouncementDAO _analyticsDAO;

    public AnnouncementRepository(SchoolDbContext context)
        : base(new AnnouncementDAO(context)) // dùng base chuẩn
    {
        _analyticsDAO = new AnnouncementDAO(context); // ép kiểu để dùng hàm riêng
    }

        public Task<Announcement> AddAnnouncementAsync(Announcement ann)=>_analyticsDAO.AddAnnouncementAsync(ann);

        public Task<bool> DeleteAnnouncementAsync(Guid id)=>_analyticsDAO.DeleteAnnouncementAsync(id);

        public Task<List<Announcement>> GetActiveAnnouncementsAsync()=> _analyticsDAO.GetActiveAnnouncementsAsync();

        public Task<List<Announcement>> GetAnnouncementsByClassAndTermAsync(Guid classId, Guid termId, Guid academicYearId)=>_analyticsDAO.GetAnnouncementsByClassAndTermAsync(classId, termId, academicYearId);

        public Task<List<Announcement>> GetAnnouncementsByClassAsync(Guid classId)=>_analyticsDAO.GetAnnouncementsByClassAsync(classId);

        public Task<List<Announcement>> GetAnnouncementsBySubjectAsync(Guid subjectId)=>_analyticsDAO.GetAnnouncementsBySubjectAsync(subjectId);

        public Task<List<Announcement>> GetAnnouncementsByTeacherAsync(Guid teacherId)=> _analyticsDAO.GetAnnouncementsByTeacherAsync(teacherId);

        public Task<List<Announcement>> GetUrgentAnnouncementsAsync()=> _analyticsDAO.GetUrgentAnnouncementsAsync();

        public Task<Announcement?> UpdateAnnouncementAsync(Announcement updated)=>_analyticsDAO.UpdateAnnouncementAsync(updated);
    }
}
