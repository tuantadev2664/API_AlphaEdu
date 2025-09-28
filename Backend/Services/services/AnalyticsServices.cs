using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using Services.interfaces;
using System;
using System.Threading.Tasks;

namespace Services.services
{
    public class AnalyticsServices : Service<Score>, IAnalyticsServices
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public AnalyticsServices(SchoolDbContext context, IAnalyticsRepository analyticsRepository)
            : base(context)
        {
            _analyticsRepository = analyticsRepository;
        }

        public async Task<StudentAnalysisDto?> AnalyzeStudentAsync(Guid studentId, Guid termId, decimal threshold = 5.0m) =>  await _analyticsRepository.AnalyzeStudentAsync(studentId, termId, threshold);
    

        // Mở rộng nếu cần:
        // public async Task<List<string>> SuggestSubjectsToSupportAsync(Guid classId, Guid termId, decimal threshold = 5.0m)
        //     => await _analyticsRepository.SuggestSubjectsToSupportAsync(classId, termId, threshold);

        // public async Task<List<StudentRiskDto>> GetAtRiskStudentsWithRiskLevelAsync(Guid classId, Guid termId, decimal threshold = 5.0m, int minSubjectsBelowThreshold = 1)
        //     => await _analyticsRepository.GetAtRiskStudentsWithRiskLevelAsync(classId, termId, threshold, minSubjectsBelowThreshold);
    }
}
