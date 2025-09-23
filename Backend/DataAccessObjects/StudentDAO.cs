using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class StudentDAO
    {
     

        public static async Task<List<User>> GetAllStudentsAsync()
        {
            using var _context = new SchoolDbContext();
            return await _context.Users
                .Where(u => u.Role == "student")
                .ToListAsync();
        }

        public static async Task<User?> GetStudentByIdAsync(Guid id)
        {
            using var _context = new SchoolDbContext();
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.Role == "student");
        }

        public static async Task<List<User>> SearchStudentsByNameAsync(string keyword)
        {
            using var _context = new SchoolDbContext();
            return await _context.Users
                .Where(u => u.Role == "student" && u.FullName.Contains(keyword))
                .ToListAsync();
        }

        public static async Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId)
        {
            using var _context = new SchoolDbContext();
            return await _context.ClassEnrollments
                .Where(ce => ce.ClassId == classId && ce.AcademicYearId == academicYearId)
                .Include(ce => ce.Student)
                .Select(ce => ce.Student)
                .ToListAsync();
        }

        public static async Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId)
        {

            using var _context = new SchoolDbContext();
            return await _context.Users
                .Where(u => u.Role == "student" && u.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
