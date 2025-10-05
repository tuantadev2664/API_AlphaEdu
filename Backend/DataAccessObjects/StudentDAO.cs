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
                    ClassId = ce.ClassId,
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

        //public async Task<List<User>> GetStudentsByClassAsync(Guid classId, Guid academicYearId)
        //{
        //    return await _context.ClassEnrollments
        //        .Where(ce => ce.ClassId == classId && ce.AcademicYearId == academicYearId)
        //        .Include(ce => ce.Student)
        //        .Select(ce => ce.Student)
        //        .ToListAsync();
        //}


        public async Task<List<object>> GetStudentsByClassAsync(Guid classId, Guid academicYearId, Guid termId)
        {
            // 🔹 1. Lấy danh sách học sinh trong lớp
            var students = await _context.ClassEnrollments
                .Where(ce => ce.ClassId == classId && ce.AcademicYearId == academicYearId)
                .Include(ce => ce.Student)
                .Select(ce => ce.Student)
                .ToListAsync();

            if (!students.Any()) return new List<object>();

            // 🔹 2. Lấy toàn bộ điểm trong lớp cho học kỳ đó
            var scores = await _context.Scores
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Where(s => s.Assessment.GradeComponent.ClassId == classId
                         && s.Assessment.GradeComponent.TermId == termId)
                .ToListAsync();

            // 🔹 3. Tính điểm trung bình cho từng học sinh
            var studentAverages = students.Select(st =>
            {
                var studentScores = scores.Where(s => s.StudentId == st.Id).ToList();

                if (!studentScores.Any())
                    return new { Student = st, Average = (decimal?)null };

                var totalWeight = studentScores.Sum(s => s.Assessment.GradeComponent.Weight);
                var weightedScore = studentScores.Sum(s => (s.Score1 ?? 0) * s.Assessment.GradeComponent.Weight);
                var avg = totalWeight > 0 ? Math.Round(weightedScore / totalWeight, 2) : (decimal?)null;

                return new { Student = st, Average = avg };
            }).ToList();

            // 🔹 4. Xếp hạng trong lớp (bỏ học sinh chưa có điểm)
            var ranked = studentAverages
                .Where(x => x.Average.HasValue)
                .OrderByDescending(x => x.Average)
                .Select((x, index) => new
                {
                    x.Student,
                    x.Average,
                    Rank = index + 1
                })
                .ToList();

            // 🔹 5. Lấy Behavior Notes cho học sinh trong lớp (theo học kỳ)
            var behaviorNotes = await _context.BehaviorNotes
                .Include(b => b.CreatedByNavigation)
                .Where(b => b.ClassId == classId && b.TermId == termId)
                .Select(b => new
                {
                    b.StudentId,
                    b.Id,
                    b.Note,
                    b.Level,
                    b.CreatedAt,
                    Teacher = new { b.CreatedBy, TeacherName = b.CreatedByNavigation.FullName }
                })
                .ToListAsync();

            // 🔹 6. Gộp dữ liệu
            var result = students.Select(st =>
            {
                var avgInfo = studentAverages.FirstOrDefault(x => x.Student.Id == st.Id);
                var rankInfo = ranked.FirstOrDefault(r => r.Student.Id == st.Id);
                var notes = behaviorNotes.Where(b => b.StudentId == st.Id).ToList();

                return new
                {
                    StudentId = st.Id,
                    StudentName = st.FullName,
                    AverageScore = avgInfo?.Average,
                    Ranking = rankInfo?.Rank,
                    BehaviorNotes = notes
                };
            }).ToList<object>();

            return result;
        }


        public async Task<List<User>> GetStudentsBySchoolAsync(Guid schoolId)
        {
            return await _dbSet
                .Where(u => u.Role == "student" && u.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
