using BusinessObjects.Models;
using DataAccessObjects.Dto;
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
        public Task<CreateBehaviorNoteResponse> AddNoteAsync(CreateBehaviorNoteRequest request) => repo.AddNoteAsync(request);

        public Task<BehaviorNote> AddNoteFromAnalysisAsync(Guid studentId, Guid classId, Guid termId, Guid teacherId, string riskLevel, string comment) => repo.AddNoteFromAnalysisAsync(studentId, classId, termId, teacherId, riskLevel, comment);

        public Task<bool> DeleteNoteAsync(Guid id) => repo.DeleteNoteAsync(id);

        public Task<object> GetAllNotesByStudentAsync(Guid studentId) => repo.GetAllNotesByStudentAsync(studentId);

        public Task<object?> GetNoteDetailAsync(Guid id) => repo.GetNoteDetailAsync(id);

        public Task<object> GetNotesByClassAsync(Guid classId, Guid termId) => repo.GetNotesByClassAsync(classId, termId);

        public Task<object> GetNotesByStudentAsync(Guid studentId, Guid termId) => repo.GetNotesByStudentAsync(studentId, termId);

        public Task<object> GetNotesByTeacherAsync(Guid teacherId, Guid termId) => repo.GetNotesByTeacherAsync(teacherId, termId);

        public Task<object> GetStudentNotesWithSummaryAsync(Guid studentId, Guid termId) => repo.GetStudentNotesWithSummaryAsync(studentId, termId);

        public Task<UpdateBehaviorNoteResponse?> UpdateNoteAsync(UpdateBehaviorNoteRequest request) => repo.UpdateNoteAsync(request);
    }
}
