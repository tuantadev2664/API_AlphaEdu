using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class AnnouncementDAO : BaseDAO<Announcement>
    {
        public AnnouncementDAO(SchoolDbContext context) : base(context) { }

        // ============================
        // 🟢 CREATE
        // ============================
        public async Task<Announcement> AddAnnouncementAsync(Announcement ann)
        {
            ann.Id = Guid.NewGuid();
            ann.CreatedAt = DateTime.UtcNow;

            await _dbSet.AddAsync(ann);
            await _context.SaveChangesAsync();
            return ann;
        }

        // ============================
        // 🟡 UPDATE
        // ============================
        public async Task<Announcement?> UpdateAnnouncementAsync(Announcement updated)
        {
            var existing = await _dbSet.FindAsync(updated.Id);
            if (existing == null) return null;

            existing.Title = updated.Title;
            existing.Content = updated.Content;
            existing.ExpiresAt = updated.ExpiresAt;
            existing.IsUrgent = updated.IsUrgent;
            existing.SubjectId = updated.SubjectId;
            existing.ClassId = updated.ClassId;

            await _context.SaveChangesAsync();
            return existing;
        }

        // ============================
        // 🔴 DELETE
        // ============================
        public async Task<bool> DeleteAnnouncementAsync(Guid id)
        {
            var existing = await _dbSet.FindAsync(id);
            if (existing == null) return false;

            _dbSet.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        // ============================
        // 🟣 GET BY CLASS + TERM + YEAR
        // ============================
        public Task<List<Announcement>> GetAnnouncementsByClassAndTermAsync(Guid classId, Guid termId, Guid academicYearId)
        {
            return _dbSet
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .Include(a => a.Sender)
                .Where(a =>
                    a.ClassId == classId &&
                    a.Class.Grade.School.AcademicYears.Any(y => y.Id == academicYearId) &&
                    (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // ============================
        // 🟢 GET BY CLASS
        // ============================
        public Task<List<Announcement>> GetAnnouncementsByClassAsync(Guid classId)
        {
            return _dbSet
                .Include(a => a.Sender)
                .Include(a => a.Subject)
                .Where(a => a.ClassId == classId &&
                            (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // ============================
        // 🟡 GET BY SUBJECT
        // ============================
        public Task<List<Announcement>> GetAnnouncementsBySubjectAsync(Guid subjectId)
        {
            return _dbSet
                .Include(a => a.Sender)
                .Include(a => a.Class)
                .Where(a => a.SubjectId == subjectId &&
                            (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // ============================
        // 🟢 GET BY TEACHER (Sender)
        // ============================
        public Task<List<Announcement>> GetAnnouncementsByTeacherAsync(Guid teacherId)
        {
            return _dbSet
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .Where(a => a.SenderId == teacherId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // ============================
        // 🔶 GET URGENT
        // ============================
        public Task<List<Announcement>> GetUrgentAnnouncementsAsync()
        {
            return _dbSet
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .Include(a => a.Sender)
.Where(a => a.IsUrgent == true && (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // ============================
        // 🟦 GET ACTIVE (còn hiệu lực)
        // ============================
        public Task<List<Announcement>> GetActiveAnnouncementsAsync()
        {
            return _dbSet
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .Include(a => a.Sender)
                .Where(a => a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // ============================
        // 🟢 GET BY ID (chuẩn hóa format)
        // ============================
        public override async Task<Announcement?> GetByIdAsync(object id)
        {
            return await _dbSet
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .Include(a => a.Sender)
                .FirstOrDefaultAsync(a => a.Id == (Guid)id);
        }
    }
}
