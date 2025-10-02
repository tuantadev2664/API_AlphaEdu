using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class BehaviorNoteDAO : BaseDAO<BehaviorNote>
    {
        public BehaviorNoteDAO(SchoolDbContext context) : base(context) { }
    
       

        // CREATE
        public  async Task<BehaviorNote> AddNoteAsync(BehaviorNote note)
        {

            using var _context = new SchoolDbContext();
            note.CreatedAt = DateTime.UtcNow;
            await _context.BehaviorNotes.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        // READ: Ghi chú theo học sinh + học kỳ
        public  async Task<List<BehaviorNote>> GetNotesByStudentAsync(Guid studentId, Guid termId)
        {

            using var _context = new SchoolDbContext();
            return await _context.BehaviorNotes
                .Where(n => n.StudentId == studentId && n.TermId == termId)
                .Include(n => n.CreatedByNavigation)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // READ: Ghi chú theo lớp + học kỳ (GV chủ nhiệm dùng)
        public  async Task<List<BehaviorNote>> GetNotesByClassAsync(Guid classId, Guid termId)
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
        public  async Task<bool> UpdateNoteAsync(BehaviorNote updatedNote)
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
        public  async Task<bool> DeleteNoteAsync(Guid id)
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
