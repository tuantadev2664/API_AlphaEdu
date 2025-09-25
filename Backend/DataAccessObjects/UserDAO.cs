using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class UserDAO : BaseDAO<User>
    {
        public UserDAO(SchoolDbContext context) : base(context) { }

        // 🔑 Login: kiểm tra email + password
        public async Task<User?> LoginAsync(string email, string password)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email && u.Phone == password);
        }

        // 📌 Lấy user theo role
        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            return await _dbSet
                .Where(u => u.Role.ToLower() == role.ToLower())
                .ToListAsync();
        }

        // 📌 Lấy user theo lớp (chủ yếu học sinh)
        public async Task<List<User>> GetStudentsByClassAsync(Guid classId)
        {
            return await _context.ClassEnrollments
                .Where(e => e.ClassId == classId)
                .Select(e => e.Student)
                .ToListAsync();
        }

        // 📌 Lấy giáo viên chủ nhiệm của lớp
        public async Task<User?> GetHomeroomTeacherAsync(Guid classId)
        {
            var cls = await _context.Classes
                .Include(c => c.HomeroomTeacher)
                .FirstOrDefaultAsync(c => c.Id == classId);

            return cls?.HomeroomTeacher;
        }

        // 📌 Lấy phụ huynh của 1 học sinh
        public async Task<List<User>> GetParentsByStudentAsync(Guid studentId)
        {
            return await _context.ParentStudents
                .Where(p => p.StudentId == studentId)
                .Select(p => p.Parent)
                .ToListAsync();
        }

        // 📌 Lấy danh sách học sinh của 1 phụ huynh
        public async Task<List<User>> GetChildrenByParentAsync(Guid parentId)
        {
            return await _context.ParentStudents
                .Where(p => p.ParentId == parentId)
                .Select(p => p.Student)
                .ToListAsync();
        }

        // 📌 Kiểm tra email đã tồn tại chưa
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        // 📌 Update role (VD: từ student → prefect, hoặc teacher → admin)
        public async Task UpdateUserRoleAsync(Guid userId, string newRole)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("Không tìm thấy user.");

            user.Role = newRole;
            await _context.SaveChangesAsync();
        }
    }
}
