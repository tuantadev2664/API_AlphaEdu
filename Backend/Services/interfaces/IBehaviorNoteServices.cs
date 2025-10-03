using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.interfaces
{
    public interface IBehaviorNoteServices : IService<BehaviorNote>
    {
        Task<BehaviorNote> AddNoteAsync(BehaviorNote note);

        // READ: Ghi chú theo học sinh + học kỳ
        Task<List<BehaviorNote>> GetNotesByStudentAsync(Guid studentId, Guid termId);

        // READ: Ghi chú theo lớp + học kỳ (GV chủ nhiệm dùng)
        Task<List<BehaviorNote>> GetNotesByClassAsync(Guid classId, Guid termId);

        // UPDATE
        Task<bool> UpdateNoteAsync(BehaviorNote updatedNote);

        // DELETE
        Task<bool> DeleteNoteAsync(Guid id);

        // READ: Lấy tất cả ghi chú của 1 học sinh (mọi kỳ)
        Task<List<BehaviorNote>> GetAllNotesByStudentAsync(Guid studentId);

        Task<List<BehaviorNote>> GetNotesByTeacherAsync(Guid teacherId, Guid termId);
        Task<BehaviorNote> AddNoteFromAnalysisAsync(
            Guid studentId,
            Guid classId,
            Guid termId,
            Guid teacherId,
            string riskLevel,
            string comment
        );
    }
}
