using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class BehaviorNoteDAO : BaseDAO<BehaviorNote>
    {
        public BehaviorNoteDAO(SchoolDbContext context) : base(context) { }

        // CREATE thường
        public async Task<BehaviorNote> AddNoteAsync(BehaviorNote note)
        {
            using var _context = new SchoolDbContext();
            note.CreatedAt = DateTime.UtcNow;
            await _context.BehaviorNotes.AddAsync(note);
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
            using var _context = new SchoolDbContext();
            var note = new BehaviorNote
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                ClassId = classId,
                TermId = termId,
                CreatedBy = teacherId,
                Note = comment,
                Level = riskLevel,   // "Thấp" | "Trung Bình" | "Cao"
                CreatedAt = DateTime.UtcNow
            };

            await _context.BehaviorNotes.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        // READ: Ghi chú theo học sinh + học kỳ
        public async Task<List<BehaviorNote>> GetNotesByStudentAsync(Guid studentId, Guid termId)
        {
            using var _context = new SchoolDbContext();
            return await _context.BehaviorNotes
                .Where(n => n.StudentId == studentId && n.TermId == termId)
                .Include(n => n.CreatedByNavigation)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // READ: Ghi chú theo lớp + học kỳ
        public async Task<List<BehaviorNote>> GetNotesByClassAsync(Guid classId, Guid termId)
        {
            using var _context = new SchoolDbContext();
            return await _context.BehaviorNotes
                .Where(n => n.ClassId == classId && n.TermId == termId)
                .Include(n => n.Student)
                .Include(n => n.CreatedByNavigation)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // ✅ READ: lấy tất cả ghi chú + phân tích tóm tắt (cho phụ huynh xem)
        public async Task<object> GetStudentNotesWithSummaryAsync(Guid studentId, Guid termId)
        {
            using var _context = new SchoolDbContext();
            var notes = await _context.BehaviorNotes
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
                Notes = notes
            };
        }

        // UPDATE
        public async Task<bool> UpdateNoteAsync(BehaviorNote updatedNote)
        {
            using var _context = new SchoolDbContext();
            var note = await _context.BehaviorNotes.FindAsync(updatedNote.Id);
            if (note == null) return false;

            note.Note = updatedNote.Note;
            note.Level = updatedNote.Level;
            note.ClassId = updatedNote.ClassId;
            note.TermId = updatedNote.TermId;
            note.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            using var _context = new SchoolDbContext();
            var note = await _context.BehaviorNotes.FindAsync(id);
            if (note == null) return false;

            _context.BehaviorNotes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BehaviorNote>> GetAllNotesByStudentAsync(Guid studentId)
        {
            using var _context = new SchoolDbContext();
            return await _context.BehaviorNotes
                .Where(n => n.StudentId == studentId)
                .Include(n => n.Term)
                .Include(n => n.CreatedByNavigation)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<BehaviorNote>> GetNotesByTeacherAsync(Guid teacherId, Guid termId)
        {
            using var _context = new SchoolDbContext();
            return await _context.BehaviorNotes
                .Where(n => n.CreatedBy == teacherId && n.TermId == termId)
                .Include(n => n.Student)
                .Include(n => n.Class)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}
