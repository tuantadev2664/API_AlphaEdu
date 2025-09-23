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
    public class AnnouncementRepository : IAnnouncementRepository
    {
        public Task<Announcement> CreateAnnouncementAsync(Announcement ann) => AnnouncementDAO.CreateAnnouncementAsync(ann);

        public Task<bool> DeleteAnnouncementAsync(Guid id)=>AnnouncementDAO.DeleteAnnouncementAsync(id);

        public Task<List<Announcement>> GetActiveAnnouncementsAsync()=> AnnouncementDAO.GetActiveAnnouncementsAsync();  

        public Task<List<Announcement>> GetAnnouncementsByClassAsync(Guid classId)=> AnnouncementDAO.GetAnnouncementsByClassAsync(classId);

        public Task<List<Announcement>> GetAnnouncementsBySenderAsync(Guid senderId) => AnnouncementDAO.GetAnnouncementsBySenderAsync(senderId);

        public Task<List<Announcement>> GetAnnouncementsBySubjectAsync(Guid subjectId)=> AnnouncementDAO.GetAnnouncementsBySubjectAsync (subjectId);

        public Task<List<Announcement>> GetUrgentAnnouncementsAsync()=> AnnouncementDAO.GetUrgentAnnouncementsAsync();

        public Task<Announcement?> UpdateAnnouncementAsync(Announcement updated)=> AnnouncementDAO.UpdateAnnouncementAsync(updated);
    }
}
