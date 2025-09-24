using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class AnnouncementDAO
    {
       

        // Tạo thông báo
        public static async Task<Announcement> CreateAnnouncementAsync(Announcement ann)
        {

            using var _context = new SchoolDbContext();
            ann.CreatedAt = DateTime.UtcNow;
            await _context.Announcements.AddAsync(ann);
            await _context.SaveChangesAsync();
            return ann;
        }

        // Cập nhật thông báo
        public static async Task<Announcement?> UpdateAnnouncementAsync(Announcement updated)
        {

            using var _context = new SchoolDbContext();
            var existing = await _context.Announcements.FindAsync(updated.Id);
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

        // Xoá thông báo
        public static async Task<bool> DeleteAnnouncementAsync(Guid id)
        {

            using var _context = new SchoolDbContext();
            var ann = await _context.Announcements.FindAsync(id);
            if (ann == null) return false;

            _context.Announcements.Remove(ann);
            await _context.SaveChangesAsync();
            return true;
        }

        // Lấy thông báo theo lớp
        public static async Task<List<Announcement>> GetAnnouncementsByClassAsync(Guid classId)
        {

            using var _context = new SchoolDbContext();
            return await _context.Announcements
                .Where(a => a.ClassId == classId &&
                            (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // Lấy thông báo theo môn
        public static async Task<List<Announcement>> GetAnnouncementsBySubjectAsync(Guid subjectId)
        {

            using var _context = new SchoolDbContext();
            return await _context.Announcements
                .Where(a => a.SubjectId == subjectId &&
                            (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // Lấy thông báo theo giáo viên (người gửi)
        public static async Task<List<Announcement>> GetAnnouncementsBySenderAsync(Guid senderId)
        {

            using var _context = new SchoolDbContext();
            return await _context.Announcements
                .Where(a => a.SenderId == senderId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // Lấy thông báo khẩn
        public static async Task<List<Announcement>> GetUrgentAnnouncementsAsync()
        {

            using var _context = new SchoolDbContext();
            return await _context.Announcements
.Where(a => a.IsUrgent == true && (a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // Lấy tất cả thông báo còn hiệu lực
        public static async Task<List<Announcement>> GetActiveAnnouncementsAsync()
        {

            using var _context = new SchoolDbContext();
            return await _context.Announcements
                .Where(a => a.ExpiresAt == null || a.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}
