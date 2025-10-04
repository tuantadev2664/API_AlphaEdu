    using BusinessObjects.Models;
    using DataAccessObjects.Dto;
using Microsoft.EntityFrameworkCore;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace DataAccessObjects
    {
        public class SubjectDAO : BaseDAO<Subject>
        {
            public SubjectDAO(SchoolDbContext context) : base(context)
            {
            }

        public async Task<List<SubjectResultDto>> GetSubjectsByStudentAsync(Guid studentId)
        {
            var query = from u in _context.Users
                        join ce in _context.ClassEnrollments on u.Id equals ce.StudentId
                        join c in _context.Classes on ce.ClassId equals c.Id
                        join ta in _context.TeacherAssignments
                            on new { ClassId = c.Id, AcademicYearId = ce.AcademicYearId }
                            equals new { ta.ClassId, ta.AcademicYearId }
                        join s in _context.Subjects on ta.SubjectId equals s.Id
                        join t in _context.Users on ta.TeacherId equals t.Id // 👈 JOIN giáo viên bộ môn
                        where u.Id == studentId
                        select new { s, c, ce, t };

            var subjects = await query
                .Distinct()
                .Select(sdata => new SubjectResultDto
                {
                    SubjectId = sdata.s.Id,
                    SubjectName = sdata.s.Name,
                    TeacherId = sdata.t.Id,
                    TeacherName = sdata.t.FullName,
                    ClassId = sdata.c.Id,
                    ClassName = sdata.c.Name,
                    Components = _context.GradeComponents
                        .Where(gc => gc.ClassId == sdata.c.Id && gc.SubjectId == sdata.s.Id)
                        .Select(gc => new GradeComponentDto
                        {
                            GradeComponentId = gc.Id,
                            ComponentName = gc.Name,
                            Kind = gc.Kind,
                            Weight = gc.Weight,
                            MaxScore = gc.MaxScore,

                            Assessments = _context.Assessments
                                .Where(a => a.GradeComponentId == gc.Id)
                                .Select(a => new AssessmentDto
                                {
                                    AssessmentId = a.Id,
                                    Title = a.Title,
                                    DueDate = a.DueDate.HasValue
                                        ? a.DueDate.Value.ToDateTime(TimeOnly.MinValue)
                                        : (DateTime?)null,
                                    Score = _context.Scores
                                        .Where(sc => sc.AssessmentId == a.Id && sc.StudentId == studentId)
                                        .Select(sc => sc.Score1)
                                        .FirstOrDefault(),
                                    IsAbsent = _context.Scores
                                        .Where(sc => sc.AssessmentId == a.Id && sc.StudentId == studentId)
                                        .Select(sc => sc.IsAbsent)
                                        .FirstOrDefault() ?? false,
                                    Comment = _context.Scores
                                        .Where(sc => sc.AssessmentId == a.Id && sc.StudentId == studentId)
                                        .Select(sc => sc.Comment)
                                        .FirstOrDefault()
                                }).ToList()
                        }).ToList()
                })
                .ToListAsync();

            return subjects;
        }


    }
}
