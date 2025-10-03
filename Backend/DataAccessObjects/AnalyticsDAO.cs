using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class AnalyticsDAO : BaseDAO<Score>
    {
        public AnalyticsDAO(SchoolDbContext context) : base(context)
        {
        }

        public async Task<StudentAnalysisDto?> AnalyzeStudentAsync(Guid studentId, Guid termId, decimal threshold = 5.0m)
        {
            var student = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == studentId && u.Role == "student");

            if (student == null) return null;

            var scores = await _dbSet
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                        .ThenInclude(gc => gc.Subject)
                .Where(s => s.StudentId == studentId && s.Assessment.GradeComponent.TermId == termId)
                .ToListAsync();

            if (!scores.Any())
            {
                return new StudentAnalysisDto
                {
                    StudentId = student.Id,
                    FullName = student.FullName,
                    Average = 0,
                    RiskLevel = "Thấp",
                    Comment = "Chưa có dữ liệu điểm."
                };
            }

            var avg = Math.Round(scores.Average(s => s.Score1 ?? 0), 2);
            var belowCount = scores.Count(s => (s.Score1 ?? 0) < threshold);

            // Đánh giá tổng thể
            string risk = "Thấp";
            string comment = "Kết quả ổn định.";

            if (avg < threshold - 2 || belowCount >= 3)
            {
                risk = "Cao";
                comment = $"⚠️ Cảnh báo nghiêm trọng: Điểm trung bình {avg:F2}, có {belowCount} môn dưới chuẩn.";
            }
            else if (avg < threshold || belowCount >= 1)
            {
                risk = "Trung Bình";
                comment = $"Điểm trung bình {avg:F2}, có {belowCount} môn dưới chuẩn. Cần cải thiện.";
            }

            // Phân tích từng môn + từng cột điểm
            var subjects = scores
     .GroupBy(s => s.Assessment.GradeComponent.Subject)
     .Select(g =>
     {
         var subject = g.Key;
         var totalWeight = g.Sum(s => s.Assessment.GradeComponent.Weight);
         var weightedScore = g.Sum(s => (s.Score1 ?? 0) * s.Assessment.GradeComponent.Weight);
         var average = totalWeight > 0 ? Math.Round(weightedScore / totalWeight, 2) : (decimal?)null;

         int below = g.Count(s => (s.Score1 ?? 0) < threshold);
         string subjectRisk = "low";
         string subjectComment = "Ổn định.";

         if (average < threshold - 2 || below >= 3)
         {
             subjectRisk = "high";
             subjectComment = $"⚠️ Điểm trung bình {average:F2}, {below} bài dưới chuẩn.";
         }
         else if (average < threshold || below >= 1)
         {
             subjectRisk = "medium";
             subjectComment = $"Điểm trung bình {average:F2}, {below} bài dưới chuẩn.";
         }

         var components = g
             .GroupBy(s => s.Assessment.GradeComponent)
             .Select(gc =>
             {
                 var avgComp = gc.Average(s => s.Score1 ?? 0);
                 var belowComp = gc.Count(s => (s.Score1 ?? 0) < threshold);

                 string compRisk = "low";
                 string compComment = "Ổn định.";
                 if (avgComp < threshold - 2 || belowComp >= 3)
                 {
                     compRisk = "high";
                     compComment = $"⚠️ Rủi ro cao: TB {avgComp:F2}, {belowComp} bài dưới chuẩn.";
                 }
                 else if (avgComp < threshold || belowComp >= 1)
                 {
                     compRisk = "medium";
                     compComment = $"Trung bình {avgComp:F2}, {belowComp} bài dưới chuẩn.";
                 }

                 return new ComponentAnalysisDto
                 {
                     GradeComponentId = gc.Key.Id,
                     GradeComponentName = gc.Key.Name,
                     Kind = gc.Key.Kind,
                     Weight = gc.Key.Weight,
                     MaxScore = gc.Key.MaxScore,
                     Average = Math.Round(avgComp, 2),
                     Count = gc.Count(),
                     BelowThresholdCount = belowComp,
                     RiskLevel = compRisk,
                     Comment = compComment,
                     Scores = gc.Select(x => x.Score1).ToList()
                 };
             }).ToList();

         return new SubjectAnalysisDto
         {
             SubjectId = subject.Id,
             SubjectName = subject.Name,
             Average = average,
             AssignmentsCount = g.Count(),
             BelowThresholdCount = below,
             RiskLevel = subjectRisk,
             Comment = subjectComment,
             Components = components
         };
     }).ToList();


            // ✅ Tóm tắt
            string summary = $"Học sinh {student.FullName} có trung bình {avg:F2} trong học kỳ này, {belowCount} môn dưới chuẩn, mức rủi ro {risk}.";

            // ✅ Trả về kết quả cuối cùng
            return new StudentAnalysisDto
            {
                StudentId = student.Id,
                FullName = student.FullName,
                Average = avg,
                BelowCount = belowCount,
                RiskLevel = risk,
                Comment = comment,
                Subjects = subjects,
                Summary = summary
            };
        }

    }
}

