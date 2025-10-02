using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class AssessmentDAO : BaseDAO<Assessment>
    {
        public AssessmentDAO(SchoolDbContext context) : base(context) { }

        public Task<List<Assessment>> GetByGradeComponentAsync(Guid gradeComponentId)
        {
            return _dbSet
                .Where(a => a.GradeComponentId == gradeComponentId)
                .OrderBy(a => a.DueDate)
                .ToListAsync();
        }

        public Task<List<Assessment>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId)
        {
            return _dbSet
                .Include(a => a.GradeComponent)
                .Where(a => a.GradeComponent.ClassId == classId
                         && a.GradeComponent.SubjectId == subjectId
                         && a.GradeComponent.TermId == termId)
                .OrderBy(a => a.DueDate)
                .ToListAsync();
        }

        public override async Task<Assessment?> GetByIdAsync(object id)
        {
            return await _dbSet
                .Include(a => a.GradeComponent)
                .FirstOrDefaultAsync(a => a.Id == (Guid)id);
        }
    }
}



