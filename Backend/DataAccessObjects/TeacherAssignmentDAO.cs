using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class TeacherAssignmentDAO : BaseDAO<TeacherAssignment>
    {
        public TeacherAssignmentDAO(SchoolDbContext context) : base(context) { }

        //public async Task<List<Class>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId)
        //{
        //    return await _context.TeacherAssignments
        //        .Where(ta => ta.TeacherId == teacherId && ta.AcademicYearId == academicYearId)
        //        .Include(ta => ta.Class)
        //        .Select(ta => ta.Class)
        //        .Distinct()
        //        .ToListAsync();
        //}

        public async Task<List<ClassWithStudentCountDto>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId)
        {
            return await _context.TeacherAssignments
                .Where(ta => ta.TeacherId == teacherId && ta.AcademicYearId == academicYearId)
                .Select(ta => new ClassWithStudentCountDto
                {
                    Id = ta.Class.Id,
                    Name = ta.Class.Name,
                    GradeId = ta.Class.GradeId,
                    HomeroomTeacherId = ta.Class.HomeroomTeacherId,
                    StudentCount = _context.ClassEnrollments
                        .Count(ce => ce.ClassId == ta.Class.Id && ce.AcademicYearId == academicYearId)
                })
                .Distinct()
                .ToListAsync();
        }
        public async Task<List<Subject>> GetSubjectsByTeacherAndClassAsync(Guid teacherId, Guid classId, Guid academicYearId)
        {
            return await _context.TeacherAssignments
                .Where(ta => ta.TeacherId == teacherId
                             && ta.ClassId == classId
                             && ta.AcademicYearId == academicYearId)
                .Include(ta => ta.Subject)
                .Select(ta => ta.Subject)
                .Distinct()
                .ToListAsync();
        }
    }
}
