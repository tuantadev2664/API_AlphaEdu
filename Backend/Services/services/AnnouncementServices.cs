using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;
using Repositories.Interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class AnnouncementServices : Service<Announcement>, IAnnouncementServices
    {
        private readonly IAnnouncementRepository _analyticsRepository;

        public AnnouncementServices(SchoolDbContext context, IAnnouncementRepository analyticsRepository)
            : base(context)
        {
            _analyticsRepository = analyticsRepository;
        }


        public Task<Announcement> AddAnnouncementAsync(Announcement ann) => _analyticsRepository.AddAnnouncementAsync(ann);

        public Task<bool> DeleteAnnouncementAsync(Guid id) => _analyticsRepository.DeleteAnnouncementAsync(id);

        public Task<List<Announcement>> GetActiveAnnouncementsAsync() => _analyticsRepository.GetActiveAnnouncementsAsync();

        public Task<List<Announcement>> GetAnnouncementsByClassAndTermAsync(Guid classId, Guid termId, Guid academicYearId) => _analyticsRepository.GetAnnouncementsByClassAndTermAsync(classId, termId, academicYearId);

        public Task<List<Announcement>> GetAnnouncementsByClassAsync(Guid classId) => _analyticsRepository.GetAnnouncementsByClassAsync(classId);

        public Task<List<Announcement>> GetAnnouncementsBySubjectAsync(Guid subjectId) => _analyticsRepository.GetAnnouncementsBySubjectAsync(subjectId);

        public Task<List<Announcement>> GetAnnouncementsByTeacherAsync(Guid teacherId) => _analyticsRepository.GetAnnouncementsByTeacherAsync(teacherId);

        public Task<List<Announcement>> GetUrgentAnnouncementsAsync() => _analyticsRepository.GetUrgentAnnouncementsAsync();

        public Task<Announcement?> UpdateAnnouncementAsync(Announcement updated) => _analyticsRepository.UpdateAnnouncementAsync(updated);
    }
}
