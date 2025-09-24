using BusinessObjects.Models;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface IAnalyticsRepository
    {
        Task<List<string>> SuggestSubjectsToSupportAsync(Guid classId, Guid termId, decimal threshold = 5.0m);

        Task<List<StudentRiskDto>> GetAtRiskStudentsWithRiskLevelAsync(
     Guid classId, Guid termId, decimal threshold = 5.0m, int minSubjectsBelowThreshold = 1);
    }
}
