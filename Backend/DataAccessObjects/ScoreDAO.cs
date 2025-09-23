using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ScoreDAO
    {
        //private readonly SchoolDbContext _context;

        //public ScoreDAO(SchoolDbContext context)
        //{
        //    _context = context;
        //}

        // CREATE
        public static async Task<Score> AddScoreAsync(Score score)
        {

            using var _context = new SchoolDbContext();
            // tránh nhập trùn
            if (await ScoreExistsAsync(score.AssessmentId, score.StudentId))
                throw new InvalidOperationException("Điểm đã tồn tại cho học sinh này trong bài kiểm tra này.");

            await _context.Scores.AddAsync(score);
            await _context.SaveChangesAsync();
            return score;
        }

        // READ (by Id)
        public static async Task<Score?> GetScoreByIdAsync(Guid id)
        {
            using var _context = new SchoolDbContext();
            return await _context.Scores
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // UPDATE
        public static async Task<Score?> UpdateScoreAsync(Score updatedScore)
        {
            using var _context = new SchoolDbContext();
            var existing = await _context.Scores.FindAsync(updatedScore.Id);
            if (existing == null) return null;

            existing.Score1 = updatedScore.Score1;
            existing.IsAbsent = updatedScore.IsAbsent;
            existing.Comment = updatedScore.Comment;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        // DELETE
        public static async Task<bool> DeleteScoreAsync(Guid id)
        {
            using var _context = new SchoolDbContext();
            var score = await _context.Scores.FindAsync(id);
            if (score == null) return false;

            _context.Scores.Remove(score);
            await _context.SaveChangesAsync();
            return true;
        }

        // LIST (all scores for a student)
        public static async Task<List<Score>> GetScoresByStudentAsync(Guid studentId)
        {
            using var _context = new SchoolDbContext();
            return await _context.Scores
                .Where(s => s.StudentId == studentId)
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .ToListAsync();
        }

        // LIST (scores by student + subject + term)
        public static async Task<List<Score>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)
        {
            using var _context = new SchoolDbContext();
            return await _context.Scores
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Where(s => s.StudentId == studentId
                            && s.Assessment.GradeComponent.SubjectId == subjectId
                            && s.Assessment.GradeComponent.TermId == termId)
                .ToListAsync();
        }

        // LIST (scores by class + term)
        public static async Task<List<Score>> GetScoresByClassAndTermAsync(Guid classId, Guid termId)
        {
            using var _context = new SchoolDbContext();
            return await _context.Scores
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Include(s => s.Student)
                .Where(s => s.Assessment.GradeComponent.ClassId == classId
                            && s.Assessment.GradeComponent.TermId == termId)
                .ToListAsync();
        }

        // CHECK (prevent duplicate score)
        public static async Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId)
        {
            using var _context = new SchoolDbContext();
            return await _context.Scores
                .AnyAsync(s => s.AssessmentId == assessmentId && s.StudentId == studentId);
        }

        // AVERAGE (per subject in a term)
        public static async Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)
        {
            using var _context = new SchoolDbContext();
            var scores = await _context.Scores
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Where(s => s.StudentId == studentId
                            && s.Assessment.GradeComponent.SubjectId == subjectId
                            && s.Assessment.GradeComponent.TermId == termId)
                .ToListAsync();

            if (!scores.Any()) return null;

            var totalWeight = scores.Sum(s => s.Assessment.GradeComponent.Weight);
            var weightedScore = scores.Sum(s => (s.Score1 ?? 0) * s.Assessment.GradeComponent.Weight);

            return totalWeight > 0 ? weightedScore / totalWeight : null;
        }

        // TRANSCRIPT (all subjects in a term for a student)
        public  static async Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId)
        {
            using var _context = new SchoolDbContext();
            var subjects = await _context.Subjects.ToListAsync();
            var transcript = new Dictionary<string, decimal?>();

            foreach (var subject in subjects)
            {
                var avg = await GetAverageScoreByStudentAndSubjectAsync(studentId, subject.Id, termId);
                transcript.Add(subject.Name, avg);
            }

            return transcript;
        }
    }
}
