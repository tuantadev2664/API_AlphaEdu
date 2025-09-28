using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using Repositories.repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class AnalyticsRepository : Repository<Score>, IAnalyticsRepository
    {
        private readonly AnalyticsDAO _analyticsDAO;

        public AnalyticsRepository(SchoolDbContext context)
            : base(new AnalyticsDAO(context)) // dùng base chuẩn
        {
            _analyticsDAO = new AnalyticsDAO(context); // ép kiểu để dùng hàm riêng
        }

        public Task<StudentAnalysisDto?> AnalyzeStudentAsync(Guid studentId, Guid termId, decimal threshold = 5.0M)
            => _analyticsDAO.AnalyzeStudentAsync(studentId, termId, threshold);

        // Nếu sau này cần thêm:
        // public Task<List<StudentRiskDto>> GetAtRiskStudentsWithRiskLevelAsync(...) 
        //     => _analyticsDAO.GetAtRiskStudentsWithRiskLevelAsync(...);
    }
}
