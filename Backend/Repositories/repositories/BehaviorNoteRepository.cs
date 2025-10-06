using BusinessObjects.Models;
using DataAccessObjects;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class BehaviorNoteRepository : Repository<BehaviorNote>, IBehaviorNoteRepository
    {
        private readonly BehaviorNoteDAO _dao;

        public BehaviorNoteRepository(SchoolDbContext context) : base(new BehaviorNoteDAO(context))
        {
            _dao = new BehaviorNoteDAO(context);
        }

        public Task<CreateBehaviorNoteResponse> AddNoteAsync(CreateBehaviorNoteRequest request, Guid teacherId) => _dao.AddNoteAsync(request, teacherId);

        public Task<BehaviorNote> AddNoteFromAnalysisAsync(Guid studentId, Guid classId, Guid termId, Guid teacherId, string riskLevel, string comment)=> _dao.AddNoteFromAnalysisAsync(studentId, classId, termId, teacherId, riskLevel, comment);

        public Task<bool> DeleteNoteAsync(Guid id)=> _dao.DeleteNoteAsync(id);

        public Task<object> GetAllNotesByStudentAsync(Guid studentId) => _dao.GetAllNotesByStudentAsync(studentId);

        public Task<object?> GetNoteDetailAsync(Guid id)=> _dao.GetNoteDetailAsync(id);

        public Task<object> GetNotesByClassAsync(Guid classId, Guid termId) => _dao.GetNotesByClassAsync(classId, termId);

        public Task<object> GetNotesByStudentAsync(Guid studentId, Guid termId)=>_dao.GetNotesByStudentAsync(studentId, termId);

        public Task<object> GetNotesByTeacherAsync(Guid teacherId, Guid termId) => _dao.GetNotesByTeacherAsync(teacherId, termId);

        public Task<object> GetStudentNotesWithSummaryAsync(Guid studentId, Guid termId)=> _dao.GetStudentNotesWithSummaryAsync(studentId,termId);

        public Task<UpdateBehaviorNoteResponse?> UpdateNoteAsync(UpdateBehaviorNoteRequest request)=> _dao.UpdateNoteAsync(request);
    }
}
