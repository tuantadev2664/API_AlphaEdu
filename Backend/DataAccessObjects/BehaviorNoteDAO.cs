using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class BehaviorNoteDAO
    {
       

        // CREATE
        public static async Task<BehaviorNote> AddNoteAsync(BehaviorNote note)
        {

            using var _context = new SchoolDbContext();
            note.CreatedAt = DateTime.UtcNow;
            await _context.BehaviorNotes.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        // READ: Ghi chú theo học sinh + học kỳ
        public static async Task<List<BehaviorNote>> GetNotesByStudentAsync(Guid studentId, Guid termId)
        {

            using var _context = new SchoolDbContext();
            return await _context.BehaviorNotes
                .Where(n => n.StudentId == studentId && n.TermId == termId)
                .Include(n => n.CreatedByNavigation)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // READ: Ghi chú theo lớp + học kỳ (GV chủ nhiệm dùng)
        public static async Task<List<BehaviorNote>> GetNotesByClassAsync(Guid classId, Guid termId)
        {

            using var _context = new SchoolDbContext();
            return await _context.BehaviorNotes
                .Where(n => n.ClassId == classId && n.TermId == termId)
                .Include(n => n.Student)
                .Include(n => n.CreatedByNavigation)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // UPDATE
        public static async Task<bool> UpdateNoteAsync(BehaviorNote updatedNote)
        {

            using var _context = new SchoolDbContext();
            var note = await _context.BehaviorNotes.FindAsync(updatedNote.Id);
            if (note == null) return false;

            note.Note = updatedNote.Note;
            note.Level = updatedNote.Level;
            note.ClassId = updatedNote.ClassId; // nếu cho phép đổi lớp
            note.TermId = updatedNote.TermId;   // nếu cho phép đổi học kỳ
            note.CreatedAt = DateTime.UtcNow;   // nên thêm trường này trong model

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public static async Task<bool> DeleteNoteAsync(Guid id)
        {

            using var _context = new SchoolDbContext();
            var note = await _context.BehaviorNotes.FindAsync(id);
            if (note == null) return false;

            _context.BehaviorNotes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        // READ: Lấy tất cả ghi chú của 1 học sinh (mọi kỳ)
        public static async Task<List<BehaviorNote>> GetAllNotesByStudentAsync(Guid studentId)
        {

            using var _context = new SchoolDbContext();
            return await _context.BehaviorNotes
                .Where(n => n.StudentId == studentId)
                .Include(n => n.Term)
                .Include(n => n.CreatedByNavigation)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}
