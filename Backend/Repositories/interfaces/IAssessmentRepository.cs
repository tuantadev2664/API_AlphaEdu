using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface IAssessmentRepository : IRepository<Assessment>
    {
        Task<List<Assessment>> GetByGradeComponentAsync(Guid gradeComponentId);
        Task<List<Assessment>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId);
    }
}


