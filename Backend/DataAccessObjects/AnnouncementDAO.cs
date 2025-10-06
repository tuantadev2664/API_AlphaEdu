using BusinessObjects.Models;
using DataAccessObjects.Dto;
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
        public async Task<TeacherAnnouncementItem> AddAnnouncementAsync(CreateAnnouncementRequest request, Guid senderId)
        {
            var ann = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                SenderId = senderId,
                ClassId = request.ClassId,
                SubjectId = request.SubjectId,
                ExpiresAt = request.ExpiresAt,
                IsUrgent = request.IsUrgent,
                CreatedAt = DateTime.UtcNow
            };

            await _dbSet.AddAsync(ann);
            await _context.SaveChangesAsync();

            // Load thêm dữ liệu liên quan để trả về đầy đủ thông tin
            ann = await _dbSet
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .Include(a => a.Sender)
                .FirstAsync(a => a.Id == ann.Id);

            return MapToDto(ann);
        }

        // ============================
        // 🟡 UPDATE
        // ============================
        public async Task<TeacherAnnouncementItem?> UpdateAnnouncementAsync(UpdateAnnouncementRequest request)
        {
            var existing = await _dbSet
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .Include(a => a.Sender)
                .FirstOrDefaultAsync(a => a.Id == request.Id);

            if (existing == null) return null;

            existing.Title = request.Title;
            existing.Content = request.Content;
            existing.ExpiresAt = request.ExpiresAt;
            existing.IsUrgent = request.IsUrgent;
            existing.SubjectId = request.SubjectId;
            existing.ClassId = request.ClassId;

            await _context.SaveChangesAsync();

            return MapToDto(existing);
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

        // ============================
        // 🧭 MAP FUNCTION
        // ============================
        private static TeacherAnnouncementItem MapToDto(Announcement a)
        {
            return new TeacherAnnouncementItem
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                CreatedAt = a.CreatedAt,
                ExpiresAt = a.ExpiresAt,
                IsUrgent = a.IsUrgent,
                ClassId = a.ClassId,
                ClassName = a.Class?.Name,
                SubjectId = a.SubjectId,
                SubjectName = a.Subject?.Name,
                SenderId = a.SenderId,
                SenderName = a.Sender?.FullName
            };
        }
    }
}
