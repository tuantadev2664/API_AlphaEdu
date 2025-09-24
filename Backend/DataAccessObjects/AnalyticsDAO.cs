using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class AnalyticsDAO
    {


        // Rule: học sinh có nguy cơ nếu điểm trung bình < threshold
        // hoặc có >= minSubjectsBelowThreshold môn dưới chuẩn
        public static async Task<List<StudentRiskDto>> GetAtRiskStudentsWithCommentsAsync(
       Guid classId, Guid termId, decimal threshold = 5.0m, int minSubjectsBelowThreshold = 1)
        {
            using var _context = new SchoolDbContext();

            var studentData = await (
                from score in _context.Scores
                join assess in _context.Assessments on score.AssessmentId equals assess.Id
                join gc in _context.GradeComponents on assess.GradeComponentId equals gc.Id
                join enrollment in _context.ClassEnrollments on score.StudentId equals enrollment.StudentId
                join u in _context.Users on score.StudentId equals u.Id
                where gc.TermId == termId
                      && enrollment.ClassId == classId
                      && u.Role == "student"
                group new { score, gc } by new { u.Id, u.FullName } into g
                select new StudentRiskDto
                {
                    StudentId = g.Key.Id,
                    FullName = g.Key.FullName,
                    Average = g.Average(x => x.score.Score1 ?? 0),
                    BelowCount = g.Count(x => (x.score.Score1 ?? 0) < threshold),
                    RiskLevel = "Low"
                }
            ).ToListAsync();

            // Xác định risk level + nhận xét
            foreach (var s in studentData)
            {
                if (s.Average < threshold - 2 || s.BelowCount >= minSubjectsBelowThreshold + 2)
                    s.RiskLevel = "High";
                else if (s.Average < threshold || s.BelowCount >= minSubjectsBelowThreshold)
                    s.RiskLevel = "Medium";
                else
                    s.RiskLevel = "Low";

                if (s.RiskLevel == "High")
                    s.Comment = $"Cảnh báo nghiêm trọng: Điểm trung bình chỉ {s.Average:F1}. Cần hỗ trợ ngay.";
                else if (s.RiskLevel == "Medium")
                    s.Comment = $"Điểm trung bình {s.Average:F1}, có {s.BelowCount} môn dưới chuẩn. Cần cải thiện.";
                else
                    s.Comment = "Kết quả ổn định.";
            }

            return studentData
                .Where(s => s.RiskLevel != "Low")
                .OrderByDescending(s => s.RiskLevel)
                .ToList();
        }


        // Gợi ý môn học cần hỗ trợ: môn có điểm trung bình thấp nhất trong lớp
        public static async Task<List<string>> SuggestSubjectsToSupportAsync(Guid classId, Guid termId, decimal threshold = 5.0m)
        {

            using var _context = new SchoolDbContext();
            var query = from score in _context.Scores
                        join assess in _context.Assessments on score.AssessmentId equals assess.Id
                        join gc in _context.GradeComponents on assess.GradeComponentId equals gc.Id
                        join enrollment in _context.ClassEnrollments
                            on score.StudentId equals enrollment.StudentId
                        where gc.TermId == termId && enrollment.ClassId == classId
                        group score by gc.SubjectId into g
                        select new
                        {
                            SubjectId = g.Key,
                            Avg = g.Average(x => x.Score1 ?? 0)
                        };

            var subjectIds = await query
                .Where(x => x.Avg < threshold)
                .OrderBy(x => x.Avg)
                .Select(x => x.SubjectId)
                .ToListAsync();

            return await _context.Subjects
                .Where(s => subjectIds.Contains(s.Id))
                .Select(s => s.Name)
                .ToListAsync();
        }
    }
}
