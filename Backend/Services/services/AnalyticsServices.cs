using DataAccessObjects;
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class AnalyticsServices : IAnalyticsServices
    {
        private readonly IAnalyticsRepository repo;

        public AnalyticsServices(IAnalyticsRepository repository) { repo = repository; }
        public Task<List<StudentRiskDto>> GetAtRiskStudentsWithRiskLevelAsync(Guid classId, Guid termId, decimal threshold = 5.0M, int minSubjectsBelowThreshold = 1) => repo.GetAtRiskStudentsWithRiskLevelAsync(classId, termId, threshold, minSubjectsBelowThreshold);

        public Task<List<string>> SuggestSubjectsToSupportAsync(Guid classId, Guid termId, decimal threshold = 5.0M)=>repo.SuggestSubjectsToSupportAsync(classId,termId,threshold);
    }
}
