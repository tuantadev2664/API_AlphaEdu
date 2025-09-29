using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class StudentDAO : BaseDAO<User>
    {
        public StudentDAO(SchoolDbContext context) : base(context)
        {
        }

        private IQueryable<User> IncludeStudentRelations(IQueryable<User> query)
        {
            return query
                .Include(u => u.School)
                .Include(u => u.ClassEnrollments)
                    .ThenInclude(ce => ce.Class)
                        .ThenInclude(c => c.Grade)
                .Include(u => u.ClassEnrollments)
                    .ThenInclude(ce => ce.AcademicYear)
                .Include(u => u.ParentStudentStudents)
                    .ThenInclude(ps => ps.Parent)
                .Include(u => u.BehaviorNoteStudents)
                .Include(u => u.ScoreStudents)
                    .ThenInclude(s => s.Assessment)
                        .ThenInclude(a => a.GradeComponent)
                            .ThenInclude(gc => gc.Subject);
        }

        // ✅ Chỉ GetById dùng DTO
        public async Task<StudentDetailDto?> GetStudentByIdAsync(Guid id)
        {
            var student = await IncludeStudentRelations(_dbSet)
                .FirstOrDefaultAsync(u => u.Id == id && u.Role == "student");

            if (student == null) return null;

            return new StudentDetailDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                SchoolName = student.School?.Name,
                Classes = student.ClassEnrollments.Select(ce => new StudentClassDto
                {
                    ClassName = ce.Class?.Name ?? "",
                    GradeName = ce.Class?.Grade?.Level ?? "",
                    AcademicYearName = ce.AcademicYear?.Name ?? ""
                }).ToList(),
                Parents = student.ParentStudentStudents.Select(ps => new ParentDto
                {
                    FullName = ps.Parent.FullName,
                    Phone = ps.Parent.Phone
                }).ToList(),
                BehaviorNotes = student.BehaviorNoteStudents
                    .Select(bn => bn.Note).ToList(),
                Scores = student.ScoreStudents.Select(s => new StudentScoreDto
                {
                    Subject = s.Assessment.GradeComponent.Subject?.Name ?? "",
                    Component = s.Assessment.GradeComponent.Name,
                    Score = s.Score1,
                    Weight = s.Assessment.GradeComponent.Weight
                }).ToList()
            };
        }

        // Giữ nguyên các hàm còn lại
        public async Task<List<User>> GetAllStudentsAsync()
        {
            return await _dbSet
                .Where(u => u.Role == "student")
                .ToListAsync();
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
