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
    public class ScoreRepository : IScoreRepository
    {
        public Task<Score> AddScoreAsync(Score score)=>ScoreDAO.AddScoreAsync(score);   

        public Task<bool> DeleteScoreAsync(Guid id)=>ScoreDAO.DeleteScoreAsync(id);

        public Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)=>ScoreDAO.GetAverageScoreByStudentAndSubjectAsync(studentId, subjectId, termId);

        public Task<Score?> GetScoreByIdAsync(Guid id)=>ScoreDAO.GetScoreByIdAsync(id);

        public Task<List<Score>> GetScoresByClassAndTermAsync(Guid classId, Guid termId)=>ScoreDAO.GetScoresByClassAndTermAsync(@classId, termId);

        public Task<List<Score>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId) => ScoreDAO.GetScoresByStudentAndSubjectAsync(studentId, subjectId, termId);

        public Task<List<Score>> GetScoresByStudentAsync(Guid studentId)=>ScoreDAO.GetScoresByStudentAsync(studentId);

        public Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId)=>ScoreDAO.GetTranscriptByStudentAsync(studentId, termId);    

        public Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId)=>ScoreDAO.ScoreExistsAsync(assessmentId, studentId);

        public Task<Score?> UpdateScoreAsync(Score updatedScore)=>ScoreDAO.UpdateScoreAsync(updatedScore);
    }
}
