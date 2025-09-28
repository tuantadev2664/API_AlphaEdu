using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAnalyticsRepository : IRepository<Score>
    {

        Task<StudentAnalysisDto?> AnalyzeStudentAsync(Guid studentId, Guid termId, decimal threshold = 5.0m);

        /// <summary>
        /// Đề xuất các môn học cần hỗ trợ thêm cho cả lớp trong 1 học kỳ.
        /// </summary>
        //    Task<List<string>> SuggestSubjectsToSupportAsync(Guid classId, Guid termId, decimal threshold = 5.0m);

        //    /// <summary>
        //    /// Lấy danh sách học sinh có nguy cơ rớt/trượt trong lớp.
        //    /// </summary>
        //    Task<List<StudentRiskDto>> GetAtRiskStudentsWithRiskLevelAsync(
        //        Guid classId, Guid termId, decimal threshold = 5.0m, int minSubjectsBelowThreshold = 1);
    }
}
