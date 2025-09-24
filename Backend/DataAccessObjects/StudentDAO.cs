using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class StudentDAO : BaseDAO<User>
    {
        public StudentDAO(SchoolDbContext context) : base(context)
        {
        }

        public async Task<List<User>> GetAllStudentsAsync()
        {
            return await _dbSet
                .Where(u => u.Role == "student")
                .ToListAsync();
        }

        public async Task<User?> GetStudentByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Id == id && u.Role == "student");
        }

        public async Task<List<User>> SearchStudentsByNameAsync(string keyword)
        {
            return await _dbSet
                .Where(u => u.Role == "student" && u.FullName.Contains(keyword))
                .ToListAsync();
        }

        public async Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId)
        {
            return await _context.ClassEnrollments
                .Where(ce => ce.ClassId == classId && ce.AcademicYearId == academicYearId)
                .Include(ce => ce.Student)
                .Select(ce => ce.Student)
                .ToListAsync();
        }

        public async Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId)
        {
            return await _dbSet
                .Where(u => u.Role == "student" && u.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
