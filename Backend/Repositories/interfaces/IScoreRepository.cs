
using BusinessObjects.Models;
using DataAccessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface IScoreRepository : IRepository<Score>
    {
        Task<List<ScoreDto>> GetScoresByStudentAsync(Guid studentId);
        Task<List<ScoreDto>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId);
        Task<List<ScoreDto>> GetScoresByClassAndTermAsync(Guid classId, Guid termId);
        Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId);
        Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId);
        Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId);
        Task<List<StudentScoresDto>> GetStudentScoresByClassAndSubjectAsync(Guid classId, Guid subjectId, Guid termId);

        Task<List<StudentRankingDto>> GetClassRankingAsync(Guid classId, Guid termId);
        Task<List<ChildFullInfoDto>> GetChildrenFullInfoAsync(Guid parentId, Guid termId);
    }
}
