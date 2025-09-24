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
    public class AnalyticsRepository : IAnalyticsRepository
    {
        public Task<List<StudentRiskDto>> GetAtRiskStudentsWithRiskLevelAsync(Guid classId, Guid termId, decimal threshold = 5.0M, int minSubjectsBelowThreshold = 1) => AnalyticsDAO.GetAtRiskStudentsWithCommentsAsync(classId, termId, threshold, minSubjectsBelowThreshold);

        public Task<List<string>> SuggestSubjectsToSupportAsync(Guid classId, Guid termId, decimal threshold = 5.0M) => AnalyticsDAO.SuggestSubjectsToSupportAsync(classId, termId, threshold);
    }
}
