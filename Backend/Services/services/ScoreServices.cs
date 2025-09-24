
﻿using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.services
{
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
    }
}
