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
    public class ScoreServices : IScoreServices
    {
        private readonly IScoreRepository repo;

        public ScoreServices(IScoreRepository repository) { repo = repository; }
        public Task<Score> AddScoreAsync(Score score)=> repo.AddScoreAsync(score);

        public Task<bool> DeleteScoreAsync(Guid id)=>repo.DeleteScoreAsync(id);

        public Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)=>repo.GetAverageScoreByStudentAndSubjectAsync(studentId, subjectId, termId);

        public Task<Score?> GetScoreByIdAsync(Guid id)=>repo.GetScoreByIdAsync(id);

        public Task<List<Score>> GetScoresByClassAndTermAsync(Guid classId, Guid termId)=> repo.GetScoresByClassAndTermAsync(@classId, termId);

        public Task<List<Score>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)=>repo.GetScoresByStudentAndSubjectAsync(studentId,subjectId,termId);    

        public Task<List<Score>> GetScoresByStudentAsync(Guid studentId)=> repo.GetScoresByStudentAsync(studentId);

        public Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId)=>repo.GetTranscriptByStudentAsync(studentId,termId);

        public Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId)=>repo.ScoreExistsAsync(assessmentId,studentId);

        public Task<Score?> UpdateScoreAsync(Score updatedScore)=>repo.UpdateScoreAsync(updatedScore);
    }
}
