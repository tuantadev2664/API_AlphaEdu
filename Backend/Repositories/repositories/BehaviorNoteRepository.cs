using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class BehaviorNoteRepository : IBehaviorNoteRepository
    {
        public Task<BehaviorNote> AddNoteAsync(BehaviorNote note)=> BehaviorNoteDAO.AddNoteAsync(note);

        public Task<bool> DeleteNoteAsync(Guid id)=>BehaviorNoteDAO.DeleteNoteAsync(id);

        public Task<List<BehaviorNote>> GetAllNotesByStudentAsync(Guid studentId)=>BehaviorNoteDAO.GetAllNotesByStudentAsync(studentId);

        public Task<List<BehaviorNote>> GetNotesByClassAsync(Guid classId, Guid termId)=>BehaviorNoteDAO.GetNotesByClassAsync(classId, termId);
        public Task<List<BehaviorNote>> GetNotesByStudentAsync(Guid studentId, Guid termId)=> BehaviorNoteDAO.GetNotesByStudentAsync(@studentId, termId);

        public Task<bool> UpdateNoteAsync(BehaviorNote updatedNote)=> BehaviorNoteDAO.UpdateNoteAsync(updatedNote);
    }
}
