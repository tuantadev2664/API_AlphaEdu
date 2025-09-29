using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class GradeComponentDAO : BaseDAO<GradeComponent>
    {
        public GradeComponentDAO(SchoolDbContext context) : base(context) { }

        public Task<List<GradeComponent>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId)
        {
            return _dbSet
                .Where(gc => gc.ClassId == classId && gc.SubjectId == subjectId && gc.TermId == termId)
                .OrderBy(gc => gc.Position)
                .ToListAsync();
        }

        public override async Task<GradeComponent?> GetByIdAsync(object id)
        {
            return await _dbSet
                .Include(gc => gc.Class)
                .Include(gc => gc.Subject)
                .Include(gc => gc.Term)
                .FirstOrDefaultAsync(gc => gc.Id == (Guid)id);
        }

        public Task<bool> ExistsByNameAsync(Guid classId, Guid subjectId, Guid termId, string name)
        {
            return _dbSet.AnyAsync(gc => gc.ClassId == classId && gc.SubjectId == subjectId && gc.TermId == termId && gc.Name.ToLower() == name.ToLower());
        }
    }
}


