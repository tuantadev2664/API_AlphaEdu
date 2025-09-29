using BusinessObjects.Models;
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

        /// <summary>
        /// Phân tích chi tiết 1 học sinh trong 1 học kỳ.
        /// </summary>
        //  public async Task<StudentAnalysisDto?> AnalyzeStudentAsync(Guid studentId, Guid termId, decimal threshold = 5.0m)
        //  {
        //      var student = await _context.Users
        //          .FirstOrDefaultAsync(u => u.Id == studentId && u.Role == "student");

        //      if (student == null) return null;

        //      var scores = await _dbSet
        //          .Include(s => s.Assessment)
        //              .ThenInclude(a => a.GradeComponent)
        //                  .ThenInclude(gc => gc.Subject)
        //          .Where(s => s.StudentId == studentId && s.Assessment.GradeComponent.TermId == termId)
        //          .ToListAsync();

        //      if (!scores.Any())
        //      {
        //          return new StudentAnalysisDto
        //          {
        //              StudentId = student.Id,
        //              FullName = student.FullName,
        //              Average = 0,
        //              RiskLevel = "Low",
        //              Comment = "Chưa có dữ liệu điểm."
        //          };
        //      }

        //      // 🔹 Tính toán điểm trung bình & số môn dưới ngưỡng
        //      var avg = Math.Round(scores.Average(s => s.Score1 ?? 0), 2);
        //      var belowCount = scores.Count(s => (s.Score1 ?? 0) < threshold);

        //      string risk = "Low";
        //      string comment = "Kết quả ổn định.";

        //      if (avg < threshold - 2 || belowCount >= 3)
        //      {
        //          risk = "High";
        //          comment = $"⚠️ Cảnh báo nghiêm trọng: Điểm trung bình {avg:F1}, có {belowCount} môn dưới chuẩn.";
        //      }
        //      else if (avg < threshold || belowCount >= 1)
        //      {
        //          risk = "Medium";
        //          comment = $"Điểm trung bình {avg:F1}, có {belowCount} môn dưới chuẩn. Cần cải thiện.";
        //      }

        //      // 🔹 Điểm từng môn (theo trọng số)
        //      var transcript = scores
        //.GroupBy(s => s.Assessment.GradeComponent.Subject.Name)
        //.ToDictionary(
        //    g => g.Key,
        //    g =>
        //    {
        //        var totalWeight = g.Sum(s => s.Assessment.GradeComponent.Weight);
        //        var weightedScore = g.Sum(s => (s.Score1 ?? 0) * s.Assessment.GradeComponent.Weight);
        //        return totalWeight > 0 ? Math.Round(weightedScore / totalWeight, 2) : (decimal?)null;
        //    });

        //      return new StudentAnalysisDto
        //      {
        //          StudentId = student.Id,
        //          FullName = student.FullName,
        //          Average = avg,
        //          BelowCount = belowCount,
        //          RiskLevel = risk,
        //          Comment = comment,
        //          Transcript = transcript
        //      };
        //  }

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

            // Phân tích từng môn
            var subjects = scores
                .GroupBy(s => s.Assessment.GradeComponent.Subject.Name)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var totalWeight = g.Sum(s => s.Assessment.GradeComponent.Weight);
                        var weightedScore = g.Sum(s => (s.Score1 ?? 0) * s.Assessment.GradeComponent.Weight);
                        var average = totalWeight > 0 ? Math.Round(weightedScore / totalWeight, 2) : (decimal?)null;

                        int below = g.Count(s => (s.Score1 ?? 0) < threshold);
                        string subjectRisk = "Thấp";
                        string subjectComment = "Ổn định.";

                        if (average < threshold - 2 || below >= 3)
                        {
                            subjectRisk = "Cao";
                            subjectComment = $"⚠️ Cần cải thiện ngay. Điểm trung bình {average:F2}, {below} bài dưới chuẩn.";
                        }
                        else if (average < threshold || below >= 1)
                        {
                            subjectRisk = "Trung Bình";
                            subjectComment = $"Điểm trung bình {average:F2}, {below} bài dưới chuẩn. Khuyến nghị ôn tập.";
                        }

                        return new SubjectAnalysisDto
                        {
                            Average = average,
                            AssignmentsCount = g.Count(),
                            BelowThresholdCount = below,
                            RiskLevel = subjectRisk,
                            Comment = subjectComment
                        };
                    });

            // Tóm tắt tổng quan
            string summary = $"Học sinh {student.FullName} có trung bình {avg:F2} trong học kỳ này, {belowCount} môn dưới chuẩn, mức rủi ro {risk}.";

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




//    // Rule: học sinh có nguy cơ nếu điểm trung bình < threshold
//    // hoặc có >= minSubjectsBelowThreshold môn dưới chuẩn
//    public static async Task<List<StudentRiskDto>> GetAtRiskStudentsWithCommentsAsync(
//   Guid classId, Guid termId, decimal threshold = 5.0m, int minSubjectsBelowThreshold = 1)
//    {
//        using var _context = new SchoolDbContext();

//        var studentData = await (
//            from score in _context.Scores
//            join assess in _context.Assessments on score.AssessmentId equals assess.Id
//            join gc in _context.GradeComponents on assess.GradeComponentId equals gc.Id
//            join enrollment in _context.ClassEnrollments on score.StudentId equals enrollment.StudentId
//            join u in _context.Users on score.StudentId equals u.Id
//            where gc.TermId == termId
//                  && enrollment.ClassId == classId
//                  && u.Role == "student"
//            group new { score, gc } by new { u.Id, u.FullName } into g
//            select new StudentRiskDto
//            {
//                StudentId = g.Key.Id,
//                FullName = g.Key.FullName,
//                Average = g.Average(x => x.score.Score1 ?? 0),
//                BelowCount = g.Count(x => (x.score.Score1 ?? 0) < threshold),
//                RiskLevel = "Low"
//            }
//        ).ToListAsync();

//        // Xác định risk level + nhận xét
//        foreach (var s in studentData)
//        {
//            if (s.Average < threshold - 2 || s.BelowCount >= minSubjectsBelowThreshold + 2)
//                s.RiskLevel = "High";
//            else if (s.Average < threshold || s.BelowCount >= minSubjectsBelowThreshold)
//                s.RiskLevel = "Medium";
//            else
//                s.RiskLevel = "Low";

//            if (s.RiskLevel == "High")
//                s.Comment = $"Cảnh báo nghiêm trọng: Điểm trung bình chỉ {s.Average:F1}. Cần hỗ trợ ngay.";
//            else if (s.RiskLevel == "Medium")
//                s.Comment = $"Điểm trung bình {s.Average:F1}, có {s.BelowCount} môn dưới chuẩn. Cần cải thiện.";
//            else
//                s.Comment = "Kết quả ổn định.";
//        }

//        return studentData
//            .Where(s => s.RiskLevel != "Low")
//            .OrderByDescending(s => s.RiskLevel)
//            .ToList();
//    }


//    // Gợi ý môn học cần hỗ trợ: môn có điểm trung bình thấp nhất trong lớp
//    public static async Task<List<string>> SuggestSubjectsToSupportAsync(Guid classId, Guid termId, decimal threshold = 5.0m)
//    {

//        using var _context = new SchoolDbContext();
//        var query = from score in _context.Scores
//                    join assess in _context.Assessments on score.AssessmentId equals assess.Id
//                    join gc in _context.GradeComponents on assess.GradeComponentId equals gc.Id
//                    join enrollment in _context.ClassEnrollments
//                        on score.StudentId equals enrollment.StudentId
//                    where gc.TermId == termId && enrollment.ClassId == classId
//                    group score by gc.SubjectId into g
//                    select new
//                    {
//                        SubjectId = g.Key,
//                        Avg = g.Average(x => x.Score1 ?? 0)
//                    };

//        var subjectIds = await query
//            .Where(x => x.Avg < threshold)
//            .OrderBy(x => x.Avg)
//            .Select(x => x.SubjectId)
//            .ToListAsync();

//        return await _context.Subjects
//            .Where(s => subjectIds.Contains(s.Id))
//            .Select(s => s.Name)
//            .ToListAsync();
//    }
//}