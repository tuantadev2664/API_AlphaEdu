using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface IScoreRepository
    {
         Task<Score> AddScoreAsync(Score score);

        Task<Score?> GetScoreByIdAsync(Guid id);

        // UPDATE
          Task<Score?> UpdateScoreAsync(Score updatedScore);

        // DELETE
          Task<bool> DeleteScoreAsync(Guid id);

        // LIST (all scores for a student)
          Task<List<Score>> GetScoresByStudentAsync(Guid studentId);

        // LIST (scores by student + subject + term)
          Task<List<Score>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId);

        // LIST (scores by class + term)
         Task<List<Score>> GetScoresByClassAndTermAsync(Guid classId, Guid termId);

        // CHECK (prevent duplicate score)
         Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId);

        // AVERAGE (per subject in a term)
         Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId);

        // TRANSCRIPT (all subjects in a term for a student)
         Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId);
    }
}
