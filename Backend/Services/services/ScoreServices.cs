using BusinessObjects.Models;
<<<<<<< HEAD
=======
using DataAccessObjects;
>>>>>>> newfix
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
using System.Text;
=======
>>>>>>> newfix
using System.Threading.Tasks;

namespace Services.services
{
<<<<<<< HEAD
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
=======
    public class ScoreServices : Service<Score>, IScoreServices
    {
        private readonly IScoreRepository _scoreRepository;

        public ScoreServices(SchoolDbContext context, IScoreRepository scoreRepository)
            : base(context) // gọi Service<Score> để dùng CRUD base
        {
            _scoreRepository = scoreRepository;
        }

        // LIST (all scores for a student)
        public Task<List<ScoreDto>> GetScoresByStudentAsync(Guid studentId)
            => _scoreRepository.GetScoresByStudentAsync(studentId);

        // LIST (scores by student + subject + term)
        public Task<List<ScoreDto>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)
            => _scoreRepository.GetScoresByStudentAndSubjectAsync(studentId, subjectId, termId);

        // LIST (scores by class + term)
        public Task<List<ScoreDto>> GetScoresByClassAndTermAsync(Guid classId, Guid termId)
            => _scoreRepository.GetScoresByClassAndTermAsync(classId, termId);

        // CHECK (prevent duplicate score)
        public Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId)
            => _scoreRepository.ScoreExistsAsync(assessmentId, studentId);

        // AVERAGE (per subject in a term)
        public Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)
            => _scoreRepository.GetAverageScoreByStudentAndSubjectAsync(studentId, subjectId, termId);

        // TRANSCRIPT (all subjects in a term for a student)
        public Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId)
            => _scoreRepository.GetTranscriptByStudentAsync(studentId, termId);
>>>>>>> newfix
    }
}
