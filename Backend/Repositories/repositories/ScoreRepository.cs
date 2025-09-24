//using BusinessObjects.Models;
//using DataAccessObjects;
//using Microsoft.EntityFrameworkCore;
//using Repositories.interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Repositories.repositories
//{
//    public class ScoreRepository : Repository<Score>, IScoreRepository
//    {
//        private readonly SchoolDbContext _context;

//        public ScoreRepository(SchoolDbContext context) : base(new BaseDAO<Score>(context))
//        {
//            _context = context;
//        }

//        // LIST (all scores for a student)
//        public async Task<List<Score>> GetScoresByStudentAsync(Guid studentId)
//        {
//            return await _context.Scores
//                .Where(s => s.StudentId == studentId)
//                .Include(s => s.Assessment)
//                    .ThenInclude(a => a.GradeComponent)
//                .ToListAsync();
//        }

//        // LIST (scores by student + subject + term)
//        public async Task<List<Score>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)
//        {
//            return await _context.Scores
//                .Include(s => s.Assessment)
//                    .ThenInclude(a => a.GradeComponent)
//                .Where(s => s.StudentId == studentId
//                         && s.Assessment.GradeComponent.SubjectId == subjectId
//                         && s.Assessment.GradeComponent.TermId == termId)
//                .ToListAsync();
//        }

//        // LIST (scores by class + term)
//        public async Task<List<Score>> GetScoresByClassAndTermAsync(Guid classId, Guid termId)
//        {
//            return await _context.Scores
//                .Include(s => s.Assessment)
//                    .ThenInclude(a => a.GradeComponent)
//                .Include(s => s.Student)
//                .Where(s => s.Assessment.GradeComponent.ClassId == classId
//                         && s.Assessment.GradeComponent.TermId == termId)
//                .ToListAsync();
//        }

//        // CHECK (prevent duplicate score)
//        public async Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId)
//        {
//            return await _context.Scores
//                .AnyAsync(s => s.AssessmentId == assessmentId && s.StudentId == studentId);
//        }

//        // AVERAGE (per subject in a term)
//        public async Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)
//        {
//            var scores = await _context.Scores
//                .Include(s => s.Assessment)
//                    .ThenInclude(a => a.GradeComponent)
//                .Where(s => s.StudentId == studentId
//                         && s.Assessment.GradeComponent.SubjectId == subjectId
//                         && s.Assessment.GradeComponent.TermId == termId)
//                .ToListAsync();

//            if (!scores.Any()) return null;

//            var totalWeight = scores.Sum(s => s.Assessment.GradeComponent.Weight);
//            var weightedScore = scores.Sum(s => (s.Score1 ?? 0) * s.Assessment.GradeComponent.Weight);

//            return totalWeight > 0 ? weightedScore / totalWeight : null;
//        }

//        // TRANSCRIPT (all subjects in a term for a student)
//        public async Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId)
//        {
//            var subjects = await _context.Subjects.ToListAsync();
//            var transcript = new Dictionary<string, decimal?>();

//            foreach (var subject in subjects)
//            {
//                var avg = await GetAverageScoreByStudentAndSubjectAsync(studentId, subject.Id, termId);
//                transcript[subject.Name] = avg;
//            }

//            return transcript;
//        }
//    }
//}


using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;
using Repositories.repositories;

public class ScoreRepository : Repository<Score>, IScoreRepository
{
    private readonly ScoreDAO _scoreDAO;

    public ScoreRepository(SchoolDbContext context) : base(new ScoreDAO(context))
    {
        _scoreDAO = new ScoreDAO(context);
    }

    public Task<List<ScoreDto>> GetScoresByStudentAsync(Guid studentId) =>
        _scoreDAO.GetScoresByStudentAsync(studentId);

    public Task<List<ScoreDto>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId) =>
        _scoreDAO.GetScoresByStudentAndSubjectAsync(studentId, subjectId, termId);

    public Task<List<ScoreDto>> GetScoresByClassAndTermAsync(Guid classId, Guid termId) =>
        _scoreDAO.GetScoresByClassAndTermAsync(classId, termId);

    public Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId) =>
        _scoreDAO.ScoreExistsAsync(assessmentId, studentId);

    public Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId) =>
        _scoreDAO.GetAverageScoreByStudentAndSubjectAsync(studentId, subjectId, termId);

    public Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId) =>
        _scoreDAO.GetTranscriptByStudentAsync(studentId, termId);
}
