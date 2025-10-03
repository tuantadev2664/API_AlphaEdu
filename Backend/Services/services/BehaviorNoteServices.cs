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
    public class BehaviorNoteServices : Service<BehaviorNote>, IBehaviorNoteServices
    {
        private readonly IBehaviorNoteRepository repo;

        public BehaviorNoteServices(SchoolDbContext context, IBehaviorNoteRepository repository)
            : base(context)
        {
            repo = repository;
        }
        public Task<BehaviorNote> AddNoteAsync(BehaviorNote note)=>repo.AddNoteAsync(note);

        public Task<BehaviorNote> AddNoteFromAnalysisAsync(Guid studentId, Guid classId, Guid termId, Guid teacherId, string riskLevel, string comment) => repo.AddNoteFromAnalysisAsync(studentId, classId, termId, teacherId, riskLevel, comment);

        public Task<bool> DeleteNoteAsync(Guid id)=>repo.DeleteNoteAsync(id);

        public Task<List<BehaviorNote>> GetAllNotesByStudentAsync(Guid studentId)=>repo.GetAllNotesByStudentAsync(studentId);

        public Task<List<BehaviorNote>> GetNotesByClassAsync(Guid classId, Guid termId)=>repo.GetNotesByClassAsync(classId, termId);

        public Task<List<BehaviorNote>> GetNotesByStudentAsync(Guid studentId, Guid termId)=> repo.GetNotesByStudentAsync(studentId, termId);

        public Task<List<BehaviorNote>> GetNotesByTeacherAsync(Guid teacherId, Guid termId)=> repo.GetNotesByTeacherAsync(teacherId, termId);

        public Task<bool> UpdateNoteAsync(BehaviorNote updatedNote)=>repo.UpdateNoteAsync(updatedNote);
    }
}
