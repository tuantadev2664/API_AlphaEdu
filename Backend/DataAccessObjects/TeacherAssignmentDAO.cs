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

        public async Task<List<Class>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId)
        {
            return await _context.TeacherAssignments
                .Where(ta => ta.TeacherId == teacherId && ta.AcademicYearId == academicYearId)
                .Include(ta => ta.Class)
                .Select(ta => ta.Class)
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
