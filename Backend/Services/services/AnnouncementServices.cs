using BusinessObjects.Models;
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class AnnouncementServices : IAnnouncementServices
    {
        private readonly IAnnouncementRepository repo;

        public AnnouncementServices(IAnnouncementRepository repository) { repo = repository; }
        public Task<Announcement> CreateAnnouncementAsync(Announcement ann) => repo.CreateAnnouncementAsync(ann);

        public Task<bool> DeleteAnnouncementAsync(Guid id)=>repo.DeleteAnnouncementAsync(id);

        public Task<List<Announcement>> GetActiveAnnouncementsAsync()=>repo.GetActiveAnnouncementsAsync();

        public Task<List<Announcement>> GetAnnouncementsByClassAsync(Guid classId)=>repo.GetAnnouncementsByClassAsync(classId);

        public Task<List<Announcement>> GetAnnouncementsBySenderAsync(Guid senderId)=>repo.GetAnnouncementsBySenderAsync(senderId);
        public Task<List<Announcement>> GetAnnouncementsBySubjectAsync(Guid subjectId)=>repo.GetAnnouncementsBySubjectAsync(subjectId);    

        public Task<List<Announcement>> GetUrgentAnnouncementsAsync()=>repo.GetUrgentAnnouncementsAsync();

        public Task<Announcement?> UpdateAnnouncementAsync(Announcement updated)=>repo.UpdateAnnouncementAsync(updated);
    }
}
