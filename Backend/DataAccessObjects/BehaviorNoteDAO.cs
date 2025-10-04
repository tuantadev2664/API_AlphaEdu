using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class BehaviorNoteDAO : BaseDAO<BehaviorNote>
    {
        public BehaviorNoteDAO(SchoolDbContext context) : base(context) { }

        // CREATE
        public async Task<BehaviorNote> AddNoteAsync(BehaviorNote note)
        {
            note.Id = Guid.NewGuid();
            note.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        // ✅ CREATE từ AI phân tích
        public async Task<BehaviorNote> AddNoteFromAnalysisAsync(
            Guid studentId,
            Guid classId,
            Guid termId,
            Guid teacherId,
            string riskLevel,
            string comment
        )
        {
            var note = new BehaviorNote
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                ClassId = classId,
                TermId = termId,
                CreatedBy = teacherId,
                Note = comment,
                Level = riskLevel,
                CreatedAt = DateTime.UtcNow
            };

            await _dbSet.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        // ✅ READ: Ghi chú theo học sinh + học kỳ
        public async Task<object> GetNotesByStudentAsync(Guid studentId, Guid termId)
        {
            var notes = await _dbSet
                .Where(n => n.StudentId == studentId && n.TermId == termId)
                .Include(n => n.CreatedByNavigation)
                .Include(n => n.Class)
                .Include(n => n.Term)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Id,
                    n.Note,
                    n.Level,
                    n.CreatedAt,
                    Class = new { n.ClassId, ClassName = n.Class.Name },
                    Term = new { n.TermId, TermName = n.Term.Code },
                    Teacher = new { n.CreatedBy, TeacherName = n.CreatedByNavigation.FullName },
                    Student = new { n.StudentId, StudentName = n.Student.FullName }
                })
                .ToListAsync();

            return notes;
        }

        // ✅ READ: Ghi chú theo lớp + học kỳ
        public async Task<object> GetNotesByClassAsync(Guid classId, Guid termId)
        {
            var notes = await _dbSet
                .Where(n => n.ClassId == classId && n.TermId == termId)
                .Include(n => n.Student)
                .Include(n => n.CreatedByNavigation)
                .Include(n => n.Term)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Id,
                    n.Note,
                    n.Level,
                    n.CreatedAt,
                    Class = new { n.ClassId, ClassName = n.Class.Name },
                    Term = new { n.TermId, TermName = n.Term.Code
                    },
                    Teacher = new { n.CreatedBy, TeacherName = n.CreatedByNavigation.FullName },
                    Student = new { n.StudentId, StudentName = n.Student.FullName }
                })
                .ToListAsync();

            return notes;
        }

        // ✅ READ: lấy tất cả ghi chú + phân tích tóm tắt (cho phụ huynh xem)
        public async Task<object> GetStudentNotesWithSummaryAsync(Guid studentId, Guid termId)
        {
            var notes = await _dbSet
                .Where(n => n.StudentId == studentId && n.TermId == termId)
                .Include(n => n.CreatedByNavigation)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            string summary = notes.Any()
                ? $"Học sinh có {notes.Count} ghi chú hành vi trong kỳ này. Mức cảnh báo mới nhất: {notes.First().Level}."
                : "Chưa có ghi chú hành vi trong kỳ này.";

            return new
            {
                StudentId = studentId,
                TermId = termId,
                Summary = summary,
                Notes = notes.Select(n => new
                {
                    n.Id,
                    n.Note,
                    n.Level,
                    n.CreatedAt,
                    Teacher = new { n.CreatedBy, TeacherName = n.CreatedByNavigation.FullName }
                })
            };
        }

        // UPDATE
        public async Task<bool> UpdateNoteAsync(BehaviorNote updatedNote)
        {
            var existing = await _dbSet.FindAsync(updatedNote.Id);
            if (existing == null) return false;

            existing.Note = updatedNote.Note;
            existing.Level = updatedNote.Level;
            existing.ClassId = updatedNote.ClassId;
            existing.TermId = updatedNote.TermId;
            existing.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            var existing = await _dbSet.FindAsync(id);
            if (existing == null) return false;

            _dbSet.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Lấy tất cả ghi chú của học sinh (mọi học kỳ)
        public async Task<object> GetAllNotesByStudentAsync(Guid studentId)
        {
            var notes = await _dbSet
                .Where(n => n.StudentId == studentId)
                .Include(n => n.Term)
                .Include(n => n.CreatedByNavigation)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Id,
                    n.Note,
                    n.Level,
                    n.CreatedAt,
                    Term = new { n.TermId, TermName = n.Term.Code },
                    Teacher = new { n.CreatedBy, TeacherName = n.CreatedByNavigation.FullName }
                })
                .ToListAsync();

            return notes;
        }

        // ✅ Lấy ghi chú do 1 giáo viên tạo trong 1 học kỳ
        public async Task<object> GetNotesByTeacherAsync(Guid teacherId, Guid termId)
        {
            var notes = await _dbSet
                .Where(n => n.CreatedBy == teacherId && n.TermId == termId)
                .Include(n => n.Student)
                .Include(n => n.Class)
                .Include(n => n.Term)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Id,
                    n.Note,
                    n.Level,
                    n.CreatedAt,
                    Class = new { n.ClassId, ClassName = n.Class.Name },
                    Term = new { n.TermId, TermName = n.Term.Code },
                    Student = new { n.StudentId, StudentName = n.Student.FullName }
                })
                .ToListAsync();

            return notes;
        }

        // ✅ GET BY ID
        public async Task<object?> GetNoteDetailAsync(Guid id)
        {
            return await _dbSet
                .Where(n => n.Id == id)
                .Include(n => n.Student)
                .Include(n => n.Class)
                .Include(n => n.Term)
                .Include(n => n.CreatedByNavigation)
                .Select(n => new
                {
                    n.Id,
                    n.Note,
                    n.Level,
                    n.CreatedAt,
                    Class = new { n.ClassId, ClassName = n.Class.Name },
                    Term = new { n.TermId, TermName = n.Term.Code },
                    Teacher = new { n.CreatedBy, TeacherName = n.CreatedByNavigation.FullName },
                    Student = new { n.StudentId, StudentName = n.Student.FullName }
                })
                .FirstOrDefaultAsync();
        }

    }
}
