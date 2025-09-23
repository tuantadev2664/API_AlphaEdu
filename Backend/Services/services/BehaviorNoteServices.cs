using BusinessObjects.Models;
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class BehaviorNoteServices : IBehaviorNoteServices
    {
        private readonly IBehaviorNoteRepository repo;

        public BehaviorNoteServices(IBehaviorNoteRepository repository) { repo = repository; }
        public Task<BehaviorNote> AddNoteAsync(BehaviorNote note)=>repo.AddNoteAsync(note);

        public Task<bool> DeleteNoteAsync(Guid id)=>repo.DeleteNoteAsync(id);

        public Task<List<BehaviorNote>> GetAllNotesByStudentAsync(Guid studentId)=>repo.GetAllNotesByStudentAsync(studentId);

        public Task<List<BehaviorNote>> GetNotesByClassAsync(Guid classId, Guid termId)=>repo.GetNotesByClassAsync(classId, termId);

        public Task<List<BehaviorNote>> GetNotesByStudentAsync(Guid studentId, Guid termId)=> repo.GetNotesByClassAsync(termId, studentId);

        public Task<bool> UpdateNoteAsync(BehaviorNote updatedNote)=>repo.UpdateNoteAsync(updatedNote);
    }
}
