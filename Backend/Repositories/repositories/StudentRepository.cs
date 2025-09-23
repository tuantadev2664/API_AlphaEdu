using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.interfaces;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class StudentRepository : Repository<User>, IStudentRepository
    {
        private readonly SchoolDbContext _context;

        public StudentRepository(SchoolDbContext context)
            : base(new BaseDAO<User>(context))  // vẫn giữ kiểu BaseDAO
        {
            _context = context;
        }

        public async Task<List<User>> GetAllStudentsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "student")
                .ToListAsync();
        }

        public async Task<User?> GetStudentByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.Role == "student");
        }

        public async Task<List<User>> SearchStudentsByNameAsync(string keyword)
        {
            return await _context.Users
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
            return await _context.Users
                .Where(u => u.Role == "student" && u.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
