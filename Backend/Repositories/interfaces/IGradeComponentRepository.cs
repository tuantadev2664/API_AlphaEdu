using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface IGradeComponentRepository : IRepository<GradeComponent>
    {
        Task<List<GradeComponent>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId);
        Task<bool> ExistsByNameAsync(Guid classId, Guid subjectId, Guid termId, string name);
    }
}


