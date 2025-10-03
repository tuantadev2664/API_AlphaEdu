
﻿using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ScoreDAO : BaseDAO<Score>
    {
        private readonly SubjectDAO _subjectDAO;
        public ScoreDAO(SchoolDbContext context) : base(context) { _subjectDAO = new SubjectDAO(context); }

        // CREATE (có check trùng)
        public override async Task AddAsync(Score score)
        {
            if (await ScoreExistsAsync(score.AssessmentId, score.StudentId))
                throw new InvalidOperationException("Điểm đã tồn tại cho học sinh này trong bài kiểm tra này.");

            await base.AddAsync(score);
        }

        // UPDATE (ghi đè để cập nhật trường riêng)
        public override async Task UpdateAsync(Score updatedScore)
        {
            var existing = await _dbSet.FindAsync(updatedScore.Id);
            if (existing == null) throw new KeyNotFoundException("Không tìm thấy điểm để cập nhật.");

            existing.Score1 = updatedScore.Score1;
            existing.IsAbsent = updatedScore.IsAbsent;
            existing.Comment = updatedScore.Comment;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // GET Score by Id (kèm include)
        public override async Task<Score?> GetByIdAsync(object id)
        {
            return await _dbSet
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(s => s.Id == (Guid)id);
        }

        // LIST: điểm theo học sinh
        public async Task<List<ScoreDto>> GetScoresByStudentAsync(Guid studentId)
        {
            return await _dbSet
                .Where(s => s.StudentId == studentId)
                .Select(s => new ScoreDto
                {
                    Id = s.Id,
                    StudentId = s.StudentId,
                    AssessmentId = s.AssessmentId,
                    Score1 = s.Score1,
                    IsAbsent = s.IsAbsent, // để bool? trong DTO
                    Comment = s.Comment,
                    AssessmentName = s.Assessment.Title,
                    GradeComponentName = s.Assessment.GradeComponent.Name,
                    Weight = s.Assessment.GradeComponent.Weight
                })
                .ToListAsync();
        }



        // LIST: điểm theo học sinh + môn + học kỳ
        public async Task<List<ScoreDto>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)
        {
            return await _dbSet
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Where(s => s.StudentId == studentId
                            && s.Assessment.GradeComponent.SubjectId == subjectId
                            && s.Assessment.GradeComponent.TermId == termId)
                .Select(s => new ScoreDto
                {
                    Id = s.Id,
                    StudentId = s.StudentId,
                    AssessmentId = s.AssessmentId,
                    Score1 = s.Score1,
                    IsAbsent = s.IsAbsent,
                    Comment = s.Comment,
                    AssessmentName = s.Assessment.Title,
                    GradeComponentName = s.Assessment.GradeComponent.Name,
                    Weight = s.Assessment.GradeComponent.Weight
                })
                .ToListAsync();
        }

        // LIST: điểm theo lớp + học kỳ
        public async Task<List<ScoreDto>> GetScoresByClassAndTermAsync(Guid classId, Guid termId)
        {
            return await _dbSet
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Include(s => s.Student)
                .Where(s => s.Assessment.GradeComponent.ClassId == classId
                            && s.Assessment.GradeComponent.TermId == termId)
                .Select(s => new ScoreDto
                {
                    Id = s.Id,
                    StudentId = s.StudentId,
                    AssessmentId = s.AssessmentId,
                    Score1 = s.Score1,
                    IsAbsent = (bool)s.IsAbsent,
                    Comment = s.Comment,
                    AssessmentName = s.Assessment.Title,
                    GradeComponentName = s.Assessment.GradeComponent.Name,
                    Weight = s.Assessment.GradeComponent.Weight
                })
                .ToListAsync();
        }

        // CHECK: điểm đã tồn tại chưa
        public async Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId)
        {
            return await _dbSet
                .AnyAsync(s => s.AssessmentId == assessmentId && s.StudentId == studentId);
        }

        // AVERAGE: điểm trung bình 1 môn trong 1 học kỳ
        public async Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId)
        {
            var scores = await _dbSet
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Where(s => s.StudentId == studentId
                            && s.Assessment.GradeComponent.SubjectId == subjectId
                            && s.Assessment.GradeComponent.TermId == termId)
                .ToListAsync();

            if (!scores.Any()) return null;

            var totalWeight = scores.Sum(s => s.Assessment.GradeComponent.Weight);
            var weightedScore = scores.Sum(s => (s.Score1 ?? 0) * s.Assessment.GradeComponent.Weight);

            return totalWeight > 0 ? weightedScore / totalWeight : null;
        }

        // TRANSCRIPT: bảng điểm tổng hợp 1 học kỳ
        public async Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId)
        {
            var subjects = await _context.Subjects.ToListAsync();
            var transcript = new Dictionary<string, decimal?>();

            foreach (var subject in subjects)
            {
                var avg = await GetAverageScoreByStudentAndSubjectAsync(studentId, subject.Id, termId);
                transcript.Add(subject.Name, avg);
            }

            return transcript;
        }

        public async Task<List<StudentScoresDto>> GetStudentScoresByClassAndSubjectAsync(Guid classId, Guid subjectId, Guid termId)
        {
            // Lấy Term để xác định năm học
            var term = await _context.Terms.FindAsync(termId);
            if (term == null) return new List<StudentScoresDto>();

            // Lấy danh sách học sinh trong lớp theo năm học
            var students = await _context.ClassEnrollments
                .Where(ce => ce.ClassId == classId && ce.AcademicYearId == term.AcademicYearId)
                .Include(ce => ce.Student)
                .Select(ce => ce.Student)
                .ToListAsync();

            // Lấy tất cả gradeComponents của môn trong kỳ
            var gradeComponents = await _context.GradeComponents
                .Where(gc => gc.ClassId == classId && gc.SubjectId == subjectId && gc.TermId == termId)
                .ToListAsync();

            // Lấy toàn bộ điểm có liên quan
            var scores = await _dbSet
                .Include(s => s.Assessment)
                .Where(s => s.Assessment.GradeComponent.ClassId == classId
                            && s.Assessment.GradeComponent.SubjectId == subjectId
                            && s.Assessment.GradeComponent.TermId == termId)
                .ToListAsync();

            // Build kết quả
            var result = students.Select(st => new StudentScoresDto
            {
                StudentId = st.Id,
                FullName = st.FullName,
                Scores = gradeComponents.Select(gc =>
                {
                    var sc = scores.FirstOrDefault(s => s.StudentId == st.Id && s.Assessment.GradeComponentId == gc.Id);

                    return new ScoreColumnDto
                    {
                        GradeComponentId = gc.Id,
                        GradeComponentName = gc.Name,
                        Kind = gc.Kind,
                        Weight = gc.Weight,
                        MaxScore = gc.MaxScore,
                        AssessmentId = sc?.AssessmentId,
                        AssessmentName = sc?.Assessment?.Title,
                        ScoreId = sc?.Id,
                        Score = sc?.Score1,
                        IsAbsent = sc?.IsAbsent ?? false,
                        Comment = sc?.Comment
                    };
                }).ToList()
            }).ToList();

            return result;
        }

        public async Task<List<StudentRankingDto>> GetClassRankingAsync(Guid classId, Guid termId)
        {
            // lấy students trong lớp theo năm học
            var term = await _context.Terms.FindAsync(termId);
            if (term == null) return new List<StudentRankingDto>();

            var students = await _context.ClassEnrollments
                .Where(ce => ce.ClassId == classId && ce.AcademicYearId == term.AcademicYearId)
                .Select(ce => ce.Student)
                .ToListAsync();

            // lấy điểm
            var scores = await _context.Scores
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.GradeComponent)
                .Where(s => s.Assessment.GradeComponent.ClassId == classId
                         && s.Assessment.GradeComponent.TermId == termId)
                .ToListAsync();

            // tính ranking
            var ranking = students.Select(st =>
            {
                var studentScores = scores.Where(s => s.StudentId == st.Id).ToList();

                if (!studentScores.Any())
                {
                    return new StudentRankingDto
                    {
                        StudentId = st.Id,
                        FullName = st.FullName,
                        Average = null,
                        Rank = null
                    };
                }

                var totalWeight = studentScores.Sum(s => s.Assessment.GradeComponent.Weight);
                var weightedScore = studentScores.Sum(s => (s.Score1 ?? 0) * s.Assessment.GradeComponent.Weight);
                var avg = totalWeight > 0 ? Math.Round(weightedScore / totalWeight, 2) : (decimal?)null;

                return new StudentRankingDto
                {
                    StudentId = st.Id,
                    FullName = st.FullName,
                    Average = avg
                };
            }).Where(r => r.Average != null)
              .OrderByDescending(r => r.Average)
              .ToList();

            // gán Rank
            for (int i = 0; i < ranking.Count; i++)
            {
                ranking[i].Rank = i + 1;
            }

            return ranking;
        }


        //public async Task<List<object>> GetChildrenFullInfoAsync(Guid parentId, Guid termId)
        //{
        //    var children = await _context.ParentStudents
        //        .Where(ps => ps.ParentId == parentId)
        //        .Include(ps => ps.Student)
        //        .Select(ps => ps.Student)
        //        .ToListAsync();

        //    var result = new List<object>();

        //    foreach (var child in children)
        //    {
        //        var scores = await GetScoresByStudentAsync(child.Id);
        //        var transcript = await GetTranscriptByStudentAsync(child.Id, termId); 

        //        var notes = await _context.BehaviorNotes
        //            .Where(b => b.StudentId == child.Id && b.TermId == termId)
        //            .ToListAsync();

        //        // Lấy thông báo dành cho lớp mà học sinh đang học
        //        var classIds = await _context.ClassEnrollments
        //    .Where(e => e.StudentId == child.Id)
        //    .Select(e => e.ClassId)
        //    .Distinct()
        //    .ToListAsync();

        //var announcements = await _context.Announcements
        //    .Where(a => classIds.Contains(a.ClassId ?? Guid.Empty))
        //    .OrderByDescending(a => a.CreatedAt)
        //    .Select(a => new {
        //        a.Id,
        //        a.Title,
        //        a.Content,
        //        a.IsUrgent,
        //        a.CreatedAt,
        //        a.ExpiresAt,
        //        a.SenderId,
        //        a.ClassId,
        //        a.SubjectId
        //    }).ToListAsync();
        //        result.Add(new
        //        {
        //            StudentId = child.Id,
        //            StudentName = child.FullName,
        //            Scores = scores,
        //            Transcript = transcript,
        //            BehaviorNotes = notes
        //        });
        //    }

        //    return result;
        //}

        //public async Task<List<ChildFullInfoDto>> GetChildrenFullInfoAsync(Guid parentId, Guid termId)
        //{
        //    var children = await _context.ParentStudents
        //        .Where(ps => ps.ParentId == parentId)
        //        .Include(ps => ps.Student)
        //        .Select(ps => ps.Student)
        //        .ToListAsync();

        //    var result = new List<ChildFullInfoDto>();

        //    foreach (var child in children)
        //    {
        //        var scores = await GetScoresByStudentAsync(child.Id); // List<ScoreDto>

        //        // FIX transcript
        //        var transcriptDict = await GetTranscriptByStudentAsync(child.Id, termId);
        //        var transcript = new TranscriptDto
        //        {
        //            TermId = termId,
        //            TermName = (await _context.Terms.FindAsync(termId))?.Code ?? "",
        //            Subjects = transcriptDict.Select(kv => new SubjectTranscriptDto
        //            {
        //                SubjectName = kv.Key,
        //                AverageScore = kv.Value ?? 0
        //            }).ToList()
        //        };

        //        var notes = await _context.BehaviorNotes
        //            .Where(b => b.StudentId == child.Id && b.TermId == termId)
        //            .Select(b => new BehaviorNoteDto
        //            {
        //                Id = b.Id,
        //                Note = b.Note ?? "",
        //                Level = b.Level,
        //                CreatedAt = b.CreatedAt,
        //                TeacherId = b.CreatedBy,
        //                TeacherName = b.CreatedByNavigation.FullName
        //            })
        //            .ToListAsync();

        //        var classIds = await _context.ClassEnrollments
        //            .Where(e => e.StudentId == child.Id)
        //            .Select(e => e.ClassId)
        //            .Distinct()
        //            .ToListAsync();

        //        var announcements = await _context.Announcements
        //            .Where(a => classIds.Contains(a.ClassId ?? Guid.Empty))
        //            .OrderByDescending(a => a.CreatedAt)
        //            .Select(a => new AnnouncementDto
        //            {
        //                Id = a.Id,
        //                Title = a.Title,
        //                Content = a.Content,
        //                IsUrgent = a.IsUrgent ?? false,  
        //                CreatedAt = a.CreatedAt,
        //                ExpiresAt = a.ExpiresAt,
        //                SenderId = a.SenderId,
        //                ClassId = a.ClassId,
        //                SubjectId = a.SubjectId
        //            })
        //            .ToListAsync();

        //        result.Add(new ChildFullInfoDto
        //        {
        //            StudentId = child.Id,
        //            StudentName = child.FullName,
        //            Scores = scores,
        //            Transcript = transcript,
        //            BehaviorNotes = notes,
        //            Announcements = announcements
        //        });
        //    }

        //    return result;
        //}


        public async Task<List<ChildFullInfoDto>> GetChildrenFullInfoAsync(Guid parentId, Guid termId)
        {
            var children = await _context.ParentStudents
                .Where(ps => ps.ParentId == parentId)
                .Include(ps => ps.Student)
                .Select(ps => ps.Student)
                .ToListAsync();

            var result = new List<ChildFullInfoDto>();

            foreach (var child in children)
            {
                var scores = await GetScoresByStudentAsync(child.Id); // List<ScoreDto>

                // FIX transcript
                var transcriptDict = await GetTranscriptByStudentAsync(child.Id, termId);
                var transcript = new TranscriptDto
                {
                    TermId = termId,
                    TermName = (await _context.Terms.FindAsync(termId))?.Code ?? "",
                    Subjects = transcriptDict.Select(kv => new SubjectTranscriptDto
                    {
                        SubjectName = kv.Key,
                        AverageScore = kv.Value ?? 0
                    }).ToList()
                };

                var notes = await _context.BehaviorNotes
                    .Where(b => b.StudentId == child.Id && b.TermId == termId)
                    .Select(b => new BehaviorNoteDto
                    {
                        Id = b.Id,
                        Note = b.Note ?? "",
                        Level = b.Level,
                        CreatedAt = b.CreatedAt,
                        TeacherId = b.CreatedBy,
                        TeacherName = b.CreatedByNavigation.FullName
                    })
                    .ToListAsync();

                var classIds = await _context.ClassEnrollments
                    .Where(e => e.StudentId == child.Id)
                    .Select(e => e.ClassId)
                    .Distinct()
                    .ToListAsync();

                var announcements = await _context.Announcements
                    .Where(a => classIds.Contains(a.ClassId ?? Guid.Empty))
                    .OrderByDescending(a => a.CreatedAt)
                    .Select(a => new AnnouncementDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Content = a.Content,
                        IsUrgent = a.IsUrgent ?? false,
                        CreatedAt = a.CreatedAt,
                        ExpiresAt = a.ExpiresAt,
                        SenderId = a.SenderId,
                        ClassId = a.ClassId,
                        SubjectId = a.SubjectId
                    })
                    .ToListAsync();

                // ✅ gọi hàm lấy danh sách môn học + điểm thành phần
                var subjects = await _subjectDAO.GetSubjectsByStudentAsync(child.Id);

                result.Add(new ChildFullInfoDto
                {
                    StudentId = child.Id,
                    StudentName = child.FullName,
                    Scores = scores,
                    Transcript = transcript,
                    BehaviorNotes = notes,
                    Announcements = announcements,
                    Subjects = subjects
                });
            }

            return result;
        }



    }
}
