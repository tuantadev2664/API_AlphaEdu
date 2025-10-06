using BusinessObjects.Models;
using DataAccessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface IBehaviorNoteRepository : IRepository<BehaviorNote>
    {
        //Task<BehaviorNote> AddNoteAsync(BehaviorNote note);
        Task<CreateBehaviorNoteResponse> AddNoteAsync(CreateBehaviorNoteRequest request, Guid teacherId);
        // READ: Ghi chú theo học sinh + học kỳ
        Task<object> GetNotesByStudentAsync(Guid studentId, Guid termId);

        // READ: Ghi chú theo lớp + học kỳ (GV chủ nhiệm dùng)
        Task<object> GetNotesByClassAsync(Guid classId, Guid termId);

        // UPDATE
        //Task<bool> UpdateNoteAsync(BehaviorNote updatedNote);
        Task<UpdateBehaviorNoteResponse?> UpdateNoteAsync(UpdateBehaviorNoteRequest request);

        // DELETE
        Task<bool> DeleteNoteAsync(Guid id);

        // READ: Lấy tất cả ghi chú của 1 học sinh (mọi kỳ)
        Task<object> GetAllNotesByStudentAsync(Guid studentId);

        Task<object> GetNotesByTeacherAsync(Guid teacherId, Guid termId);

        Task<BehaviorNote> AddNoteFromAnalysisAsync(
              Guid studentId,
              Guid classId,
              Guid termId,
              Guid teacherId,
              string riskLevel,
              string comment
          );
        Task<object> GetStudentNotesWithSummaryAsync(Guid studentId, Guid termId);
        Task<object?> GetNoteDetailAsync(Guid id);
        }
}
